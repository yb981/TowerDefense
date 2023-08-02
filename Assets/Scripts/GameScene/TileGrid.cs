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

    private void Awake()
    {
        tileGrid = new Grid<GridTileObject>( width,  height,  cellSize,  transform.position,  GridBuildingSystem.FieldType.tile, (Grid<GridTileObject> g, int x, int y) => new GridTileObject(g, x, y));
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            tileGrid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            Debug.Log("trying to set at: "+ x +","+y);
            GridTileObject currentObject = tileGrid.GetGridObject(UtilsClass.GetMouseWorldPosition());
            if(currentObject != null)
            {
                if(currentObject.GetTransform() == null)
                {
                    Transform newTile = Instantiate(testTile,tileGrid.GetWorldPosition(x,y),Quaternion.identity);
                    currentObject.SetTransfrom(newTile);
                }
            }
        }
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
