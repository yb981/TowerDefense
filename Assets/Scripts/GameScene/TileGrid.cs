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

    [Header("test")]
    [SerializeField] private Transform testTile;
    [Header("StartTile")]
    [SerializeField] private Transform startTile;
    [SerializeField] private int StartX;
    [SerializeField] private int StartY;
    [Header("BossTile")]
    [SerializeField] private Transform bossTile;
    [SerializeField] private int maxRangeFromStart;
    [SerializeField] private int minRangeFromStart;
    private int BossX;
    private int BossY;
    
    private Transform buildTile;

    private GridTileObject currentObject;
    private Transform ghostObject;
    private bool building;

    private void Awake()
    {
        tileGrid = new Grid<GridTileObject>(width, height, cellSize, transform.position, (Grid<GridTileObject> g, int x, int y) => new GridTileObject(g, x, y));
    }

    private void Start()
    {
        TileSelection tileSelection = FindObjectOfType<TileSelection>();
        tileSelection.OnTileSelected += TileSelection_OnTileSelected;

        gridBuildingSystem = GetComponent<GridBuildingSystem>();

        CreateTilesInGrid();

    }

    private void CreateTilesInGrid()
    {
        Transform startT = Instantiate(startTile, tileGrid.GetWorldPosition(StartX,StartY), Quaternion.identity);
        currentObject = tileGrid.GetGridObject(StartX,StartY);
        StartCoroutine(CreateStartTileEndOfFrameAndBossTile(startT,StartX,StartY));
    }

    private IEnumerator CreateStartTileEndOfFrameAndBossTile(Transform tile, int x, int y)
    {
        yield return null;

        PlaceNewTile(tile,StartX,StartY);

        // Random Boss Coords
        BossX = (int) (StartX + ((UnityEngine.Random.Range(0,2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart,maxRangeFromStart));
        BossY = (int) (StartY + ((UnityEngine.Random.Range(0,2) - 0.5) * 2) * UnityEngine.Random.Range(minRangeFromStart,maxRangeFromStart));

        Transform bossT = Instantiate(bossTile, tileGrid.GetWorldPosition(BossX,BossY), Quaternion.identity);
        currentObject = tileGrid.GetGridObject(BossX,BossY);
        PlaceNewTile(bossT,BossX,BossY);
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        StartBuilding(e.tilePrefab);
    }

    private void StartBuilding(Transform newTile)
    {
        building = true;
        ghostObject = Instantiate(newTile, new Vector3(0, 0, 0), Quaternion.identity);
        StartCoroutine("BuildingTiles");
    }

    private IEnumerator BuildingTiles()
    {
        while (building)
        {
            // Update Position
            // TODO only update position if can build in that area
            // there is an entry point from existing tile
            tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            if (ghostObject.transform.position != tileGrid.GetWorldPosition(x, y))
            {
                ghostObject.transform.position = tileGrid.GetWorldPosition(x, y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ghostObject.GetComponent<Tile>().RotateTile();
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryPlacingTile();
            }
            yield return null;
        }
    }

    private void TryPlacingTile()
    {
        currentObject = tileGrid.GetGridObject(UtilsClass.GetMouseWorldPosition());
        if (currentObject != null)
        {
            if (CanConnectTile())
            {
                PlaceNewTileAndContinue();
                building = false;
            }
        }
    }

    private void PlaceNewTileAndContinue()
    {
        tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
        PlaceNewTile(ghostObject,x,y);
        LevelManager.instance.EndTileBuild();
    }

    private void PlaceNewTile(Transform newObject, int x, int y)
    {
        currentObject.SetTransfrom(newObject);
        Debug.Log("currentObj: "+currentObject.GetTransform());
        ApplyBuildingArea(newObject, x,y);
        ApplyConnectionNodes(newObject, x,y);
        SetSpawnersOfNewTile(newObject);
        OnTileBuilt?.Invoke();
        Debug.Log("x: "+x+ ", y: "+y+":"+tileGrid.GetGridObject(x,y).GetTransform());
    }

    private void SetSpawnersOfNewTile(Transform newObject)
    {
        TileSpawnManager tileSpawnManager = newObject.GetComponentInChildren<TileSpawnManager>();
        tileSpawnManager?.InitializeSpawners();
    }

    private void ApplyBuildingArea(Transform newObject, int x, int y)
    {
        BuildTileData buildArea = newObject.GetComponent<Tile>().GetBuildArea();

        int originX = x*cellSize;
        int originY = y*cellSize;
        for (int i = 0; i < buildArea.rows.Length; i++)
        {
            for (int j = 0; j < buildArea.rows[i].row.Length; j++)
            {
                gridBuildingSystem.SetTypeOfCell(buildArea.rows[i].row[j],j+originX,i+originY);
            }
        }
    }

    private void ApplyConnectionNodes(Transform newObject, int x, int y)
    {
        Tile newTile = newObject.GetComponent<Tile>();

        // Check South
        GridTileObject nextTile = tileGrid.GetGridObject(x, y-1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetNorthNode();
            Tile.TileNode ghostNode = newTile.GetSouthNode();

            if(node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check North
        nextTile = tileGrid.GetGridObject(x, y+1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetSouthNode();
            Tile.TileNode ghostNode = newTile.GetNorthNode();

            if(node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check West
        nextTile = tileGrid.GetGridObject(x-1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetEastNode();
            Tile.TileNode ghostNode = newTile.GetWestNode();

            if(node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

        // Check East
        nextTile = tileGrid.GetGridObject(x+1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetWestNode();
            Tile.TileNode ghostNode = newTile.GetEastNode();

            if(node.entry == true)
            {
                node.connectionTile = newTile.transform;
                ghostNode.connectionTile = nextTile.GetTransform();
            }
        }

    }

    private bool CanConnectTile()
    {
        // Check if current cell is empty
        GridTileObject currentHover = tileGrid.GetGridObject(UtilsClass.GetMouseWorldPosition());
        if (currentHover != null && currentHover.GetTransform() != null)
        {
            return false;
        }

        return NodesFitInAllDirections();

    }

    private bool NodesFitInAllDirections()
    {
        int connections = 0;
        tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);

        // Check if can fit, compare connections
        Tile ghostTile = ghostObject.GetComponent<Tile>();

        // Check South
        GridTileObject nextTile = tileGrid.GetGridObject(x, y-1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetNorthNode();
            Tile.TileNode ghostNode = ghostTile.GetSouthNode();

            if (node.entry != ghostNode.entry) 
            {
                return false;
            }

            if(node.entry == true)
            {
                connections++;
            }
        }

        // Check North
        nextTile = tileGrid.GetGridObject(x, y+1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetSouthNode();
            Tile.TileNode ghostNode = ghostTile.GetNorthNode();

            if (node.entry != ghostNode.entry) return false;

            if(node.entry == true) connections++;
        }

        // Check West
        nextTile = tileGrid.GetGridObject(x-1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetEastNode();
            Tile.TileNode ghostNode = ghostTile.GetWestNode();

            if (node.entry != ghostNode.entry) return false;
            if(node.entry == true) connections++;
        }

        // Check East
        nextTile = tileGrid.GetGridObject(x+1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetWestNode();
            Tile.TileNode ghostNode = ghostTile.GetEastNode();

            if (node.entry != ghostNode.entry) return false;
            if(node.entry == true) connections++;
        }

        if (connections != 0) return true;

        return false;
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
        return tileGrid.GetCellCenter(StartX,StartY);
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
