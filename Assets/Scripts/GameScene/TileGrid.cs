using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

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
        currentObject = tileGrid.GetGridObject(x,y);
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

    public void PlaceNewTile(Transform newObject, int x, int y)
    {
        currentObject = tileGrid.GetGridObject(x,y);
        currentObject.SetTransfrom(newObject);

        SetSubTiles(newObject, x, y);
        SetConnectionNodes(newObject, x, y);
        SetSpawners(newObject);
        OnTileBuilt?.Invoke();
    }

    private void SetSpawners(Transform newObject)
    {
        TileSpawnManager tileSpawnManager = newObject.GetComponentInChildren<TileSpawnManager>();
        tileSpawnManager?.InitializeSpawners();
    }

    private void SetSubTiles(Transform newObject, int x, int y)
    {
        BuildTileData buildArea = newObject.GetComponent<Tile>().GetBuildArea();

        int originX = x * cellSize;
        int originY = y * cellSize;
        for (int i = 0; i < buildArea.rows.Length; i++)
        {
            for (int j = 0; j < buildArea.rows[i].row.Length; j++)
            {
                gridBuildingSystem.SetTypeOfCell(buildArea.rows[i].row[j], j + originX, i + originY);
            }
        }
    }

    private void SetConnectionNodes(Transform newObject, int x, int y)
    {
        Tile newTile = newObject.GetComponent<Tile>();

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

    private bool CanConnectTile(Transform newObject, int x, int y)
    {
        // Check if current cell is empty
        GridTileObject currentHover = tileGrid.GetGridObject(x,y);
        if (currentHover != null && currentHover.GetTransform() != null)
        {
            return false;
        }
        return NodesFitInAllDirections(newObject,x,y);
    }

    private bool NodesFitInAllDirections(Transform newObject, int x, int y)
    {
        int connections = 0;

        // Check if can fit, compare connections
        Tile ghostTile = newObject.GetComponent<Tile>();

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

    public Vector3 GetStartTileCenterPosition()
    {
        return tileGrid.GetCellCenter(StartX, StartY);
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
    }
}
