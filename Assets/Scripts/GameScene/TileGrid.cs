using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TileGrid : MonoBehaviour
{
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

    private GridTileObject currentObject;
    private Transform ghostObject;
    private bool building;

    private void Awake()
    {
        tileGrid = new Grid<GridTileObject>(width, height, cellSize, transform.position, GridBuildingSystem.FieldType.tile, (Grid<GridTileObject> g, int x, int y) => new GridTileObject(g, x, y));
    }

    private void Start()
    {
        LevelManager.instance.OnLevelPhaseTileBuild += LevelManager_OnLevelPhaseTileBuild;
        if (LevelManager.instance.GetLevelPhase() == LevelManager.LevelPhase.tilebuild) LevelManager_OnLevelPhaseTileBuild();

        GridTileObject startGridTileObject = tileGrid.GetGridObject(StartX,StartY);
        startGridTileObject.SetTransfrom(Instantiate(startTile, tileGrid.GetWorldPosition(StartX,StartY), Quaternion.identity));
    }

    private void LevelManager_OnLevelPhaseTileBuild()
    {
        building = true;
        ghostObject = Instantiate(testTile, new Vector3(0, 0, 0), Quaternion.identity);
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
                ghostObject.GetComponent<Tile>().TurnTile();
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
        //Transform newTile = Instantiate(testTile,tileGrid.GetWorldPosition(x,y),Quaternion.identity);
        //currentObject.SetTransfrom(newTile);
        currentObject.SetTransfrom(ghostObject);
        LevelManager.instance.EndTileBuild();
    }

    private bool CanConnectTile()
    {
        Debug.Log("seeing if can connect tile");
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

            connections++;
        }
        Debug.Log("South aligns");

        // Check North
        nextTile = tileGrid.GetGridObject(x, y+1);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetSouthNode();
            Tile.TileNode ghostNode = ghostTile.GetNorthNode();

            if (node.entry != ghostNode.entry) return false;
            connections++;
        }
        Debug.Log("North aligns");

        // Check West
        nextTile = tileGrid.GetGridObject(x-1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetEastNode();
            Tile.TileNode ghostNode = ghostTile.GetWestNode();

            if (node.entry != ghostNode.entry) return false;
            connections++;
        }
        Debug.Log("West aligns");

        // Check East
        nextTile = tileGrid.GetGridObject(x+1, y);
        if (nextTile != null && nextTile.GetTransform() != null)
        {
            Tile.TileNode node = nextTile.GetTransform().GetComponent<Tile>().GetWestNode();
            Tile.TileNode ghostNode = ghostTile.GetEastNode();

            if (node.entry != ghostNode.entry) return false;
            connections++;
        }
        Debug.Log("East aligns");

        Debug.Log("connections: "+ connections);
        if (connections != 0) return true;

        return false;
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
