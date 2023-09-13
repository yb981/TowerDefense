using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.Tilemaps;

public class TileGrid : MonoBehaviour
{
    public event Action OnTileBuilt;

    private GridBuildingSystem gridBuildingSystem;
    Grid<GridTileObject> tileGrid;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellSize;

    private int StartX;
    private int StartY;

    private Transform buildTile;

    private GridTileObject currentObject;
    private List<GridTileObject> allOpenEndTiles = new List<GridTileObject>();

    private void Awake()
    {
        tileGrid = new Grid<GridTileObject>(width, height, cellSize, transform.position, (Grid<GridTileObject> g, int x, int y) => new GridTileObject(g, x, y));
    }

    private void Start()
    {
        gridBuildingSystem = GetComponent<GridBuildingSystem>();
    }

    public bool TryPlacingConnectionTile(Transform instantiatedTile, int x, int y)
    {
        currentObject = tileGrid.GetGridObject(x, y);
        if (currentObject != null)
        {
            if (CanConnectTile(instantiatedTile, x, y))
            {
                PlaceNewTile(instantiatedTile, x, y);
                return true;
            }
        }
        return false;
    }

    public void PlaceNewTile(Transform instantiatedTile, int x, int y)
    {
        currentObject = tileGrid.GetGridObject(x, y);
        currentObject.SetTransfrom(instantiatedTile);

        SetSubTiles(instantiatedTile, x, y);
        SetConnectionNodes(instantiatedTile, x, y);
        SetSpawners(instantiatedTile);
        UpdateOpenEnds();
        OnTileBuilt?.Invoke();
    }

    private void SetSpawners(Transform instantiatedTile)
    {
        TileSpawnManager tileSpawnManager = instantiatedTile.GetComponentInChildren<TileSpawnManager>();
        tileSpawnManager?.InitializeSpawners();
    }

    private void SetSubTiles(Transform instantiatedTile, int x, int y)
    {
        Tile tile = instantiatedTile.GetComponent<Tile>();
        BuildTileData buildArea = tile.GetBuildArea();
        MainTileEffect mainTileEffect = tile.GetTileEffect();
        Tilemap tilemap = tile.GetTilemap();
        int[,] heightMap = tile.GetHeightMap();

        int originX = x * cellSize;
        int originY = y * cellSize;
        for (int i = 0; i < buildArea.rows.Length; i++)
        {
            for (int j = 0; j < buildArea.rows[i].row.Length; j++)
            {
                gridBuildingSystem.SetTypeOfCell(buildArea.rows[i].row[j], j + originX, i + originY);
                gridBuildingSystem.SetMainEffectOfCell(mainTileEffect, j + originX, i + originY);

                if (buildArea.rows[i].row[j] == GridBuildingSystem.FieldType.building)
                {
                    // Set Random height
                    int height = heightMap[i,j];
                    gridBuildingSystem.SetSubTileGroundLevel(tilemap, height, j + originX, i + originY);
                }
            }
        }
    } 

    private void SetConnectionNodes(Transform instantiatedTile, int x, int y)
    {
        Tile newTile = instantiatedTile.GetComponent<Tile>();

        // Check South
        GridTileObject nextTile = tileGrid.GetGridObject(x, y - 1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetNorthNode();
            Tile.TileNode ghostNode = newTile.GetSouthNode();

            if (node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check North
        nextTile = tileGrid.GetGridObject(x, y + 1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetSouthNode();
            Tile.TileNode ghostNode = newTile.GetNorthNode();

            if (node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check West
        nextTile = tileGrid.GetGridObject(x - 1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetEastNode();
            Tile.TileNode ghostNode = newTile.GetWestNode();

            if (node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check East
        nextTile = tileGrid.GetGridObject(x + 1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetWestNode();
            Tile.TileNode ghostNode = newTile.GetEastNode();

            if (node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }
    }

    private bool CanConnectTile(Transform instantiatedTile, int x, int y)
    {
        // Check if current cell is empty
        GridTileObject currentHover = tileGrid.GetGridObject(x, y);
        if (currentHover != null && currentHover.GetTransform() != null)
        {
            return false;
        }
        return NodesFitInAllDirections(instantiatedTile, x, y);
    }

    private bool NodesFitInAllDirections(Transform instantiatedTile, int x, int y)
    {
        int connections = 0;

        // Check if can fit, compare connections
        Tile ghostTile = instantiatedTile.GetComponent<Tile>();

        // Check South
        GridTileObject nextTile = tileGrid.GetGridObject(x, y - 1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetNorthNode();
            Tile.TileNode ghostNode = ghostTile.GetSouthNode();

            if (node.entry != ghostNode.entry)
            {
                return false;
            }

            if (node.entry == true)
            {
                connections++;
            }
        }

        // Check North
        nextTile = tileGrid.GetGridObject(x, y + 1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetSouthNode();
            Tile.TileNode ghostNode = ghostTile.GetNorthNode();

            if (node.entry != ghostNode.entry) return false;

            if (node.entry == true) connections++;
        }

        // Check West
        nextTile = tileGrid.GetGridObject(x - 1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetEastNode();
            Tile.TileNode ghostNode = ghostTile.GetWestNode();

            if (node.entry != ghostNode.entry) return false;
            if (node.entry == true) connections++;
        }

        // Check East
        nextTile = tileGrid.GetGridObject(x + 1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetWestNode();
            Tile.TileNode ghostNode = ghostTile.GetEastNode();

            if (node.entry != ghostNode.entry) return false;
            if (node.entry == true) connections++;
        }

        if (connections != 0) return true;

        return false;
    }


    public bool TryPlacingSoloTile(Transform tilePrefab, int x, int y)
    {
        Debug.Log("trying to build Solotile at: " + x + "," + y);

        currentObject = tileGrid.GetGridObject(x, y);
        if (currentObject != null)
        {
            if (NoTilesAdjustent(x, y))
            {
                Transform instantiatedTile = Instantiate(tilePrefab, tileGrid.GetWorldPosition(x, y), Quaternion.identity);
                PlaceNewTile(instantiatedTile, x, y);
                return true;
            }
        }
        return false;
    }

    private bool NoTilesAdjustent(int x, int y)
    {
        Transform neighborTile;
        GridTileObject neighborObj;
        // Check North
        neighborObj = tileGrid.GetGridObject(x, y + 1);
        if (neighborObj != null)
        {
            neighborTile = neighborObj.GetTransform();
            if (neighborTile != null) return false;
        }

        // Check South
        neighborObj = tileGrid.GetGridObject(x, y - 1);
        if (neighborObj != null)
        {
            neighborTile = neighborObj.GetTransform();
            if (neighborTile != null) return false;
        }

        // Check East
        neighborObj = tileGrid.GetGridObject(x + 1, y);
        if (neighborObj != null)
        {
            neighborTile = neighborObj.GetTransform();
            if (neighborTile != null) return false;
        }

        // Check West
        neighborObj = tileGrid.GetGridObject(x - 1, y);
        if (neighborObj != null)
        {
            neighborTile = neighborObj.GetTransform();
            if (neighborTile != null) return false;
        }

        return true;
    }

    private List<GridTileObject> UpdateOpenEnds()
    {
        List<GridTileObject> allTiles = new List<GridTileObject>();
        // All Tiles
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridTileObject current = tileGrid.GetGridObject(i, j);
                
                if (current.GetTransform() != null && TileConnected(i,j)) allTiles.Add(current);
            }
        }

        allOpenEndTiles = new List<GridTileObject>();
        // Only pick open end tiles
        foreach (GridTileObject current in allTiles)
        {
            Tile.TileNode[] openings = current.GetTransform().GetComponent<Tile>().GetOpeningNodes();
            foreach (Tile.TileNode currentOpening in openings)
            {
                if (currentOpening.entry && currentOpening.connectionTile == null)
                {
                    allOpenEndTiles.Add(current);
                    break;
                }
            }
        }

        return allOpenEndTiles;
    }

    public bool TileConnected(int x, int y)
    {
        if(x == StartX && y == StartY) return true;
        Tile currentTile = tileGrid.GetGridObject(x,y).GetTransform().GetComponent<Tile>();
        bool connected = false;
        foreach ( var node in currentTile.GetOpeningNodes())
        {
            if(node.entry && node.connectionTile != null) connected = true;
        }
        return connected;
    }

    public List<GridTileObject> GetAllOpenEnds()
    {
        return allOpenEndTiles;
    }

    public Vector3 GetWorldPositionOfCell(int x, int y)
    {
        return tileGrid.GetWorldPosition(x,y);
    }

    public Tile GetTileOfPosition(Vector3 location)
    {
        return tileGrid.GetGridObject(location).GetTransform().GetComponent<Tile>();
    }

    public int GetCellSize()
    {
        return cellSize;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public Grid<GridTileObject> GetTileGrid()
    {
        return tileGrid;
    }

    public void SetStartTile(int x, int y)
    {
        StartX = x;
        StartY = y;
    }

    public class GridTileObject
    {
        private Grid<GridTileObject> grid;
        private int x;
        private int y;
        private Transform tile;

        public GridTileObject(Grid<GridTileObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTransfrom(Transform tile)
        {
            this.tile = tile;
            grid.TriggerGridObjectChanged(x, y);
        }

        public Transform GetTransform()
        {
            return tile;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(x, y);
        }
    }
}
