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

    private GridTileObject currentObject;
    private Transform ghostObject;
    private bool building;

    private void Awake()
    {
        tileGrid = new Grid<GridTileObject>( width,  height,  cellSize,  transform.position,  GridBuildingSystem.FieldType.tile, (Grid<GridTileObject> g, int x, int y) => new GridTileObject(g, x, y));
    }

    private void Start() 
    {
        LevelManager.instance.OnLevelPhaseTileBuild += LevelManager_OnLevelPhaseTileBuild;    
        if(LevelManager.instance.GetLevelPhase() == LevelManager.LevelPhase.tilebuild) LevelManager_OnLevelPhaseTileBuild();
    }

    private void LevelManager_OnLevelPhaseTileBuild()
    {
        building = true;
        ghostObject = Instantiate(testTile,new Vector3(0,0,0),Quaternion.identity);
        StartCoroutine("BuildingTiles");
    }

    private IEnumerator BuildingTiles()
    {
        while(building)
        {
            // Update Position
            // TODO only update position if can build in that area
            // there is an entry point from existing tile
            tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            if( ghostObject.transform.position != tileGrid.GetWorldPosition(x,y))
            {
                ghostObject.transform.position = tileGrid.GetWorldPosition(x,y);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ghostObject.GetComponent<Tile>().TurnTile();
            }

            if(Input.GetMouseButtonDown(0))
            {
                //tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
                currentObject = tileGrid.GetGridObject(UtilsClass.GetMouseWorldPosition());
                if(currentObject != null)
                {
                    if(currentObject.GetTransform() == null)
                    {
                        PlaceNewTileAndContinue();
                        building = false;
                    }
                }
            }
            yield return null;
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
