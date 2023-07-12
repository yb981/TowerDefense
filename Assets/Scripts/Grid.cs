using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs 
    {
        public int x;
        public int y;
    }

    private Vector3 origin;
    private int width;
    private int height;
    private float cellSize;
    GridBuildingSystem.FieldType type;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 origin, GridBuildingSystem.FieldType type ,Func<Grid<TGridObject>, int , int , TGridObject> createGridObject)
    {
        this.origin = origin;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.type = type;

        gridArray = new TGridObject[width,height];
        debugTextArray = new TextMesh[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x,y] = createGridObject(this, x, y);
            }   
        }

        bool showDebug = true;
        if(showDebug){
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Vector3 cellPos = GetWorldPosition(x,y);
                    Vector3 offset = new Vector3(cellSize,cellSize) * 0.5f;
                    debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y]?.ToString(),null,cellPos + offset,3,Color.white,TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(width,0),GetWorldPosition(width,height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0,height),GetWorldPosition(width,height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => { 
                debugTextArray[eventArgs.x,eventArgs.y].text = GetGridObject(eventArgs.x,eventArgs.y)?.ToString(); 
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x,y)*cellSize)+origin;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition -origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition -origin).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
            debugTextArray[x,y].text = gridArray[x,y].ToString();
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x,y,value);
        TriggerGridObjectChanged(x, y);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x,y];
        }else{
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x,y);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{ x = x, y = y});
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        float centerX = origin.x + x * cellSize + cellSize * 0.5f;
        float centerY = origin.y + y * cellSize + cellSize * 0.5f;

        return new Vector3(centerX,centerY,0);
    }

    public GridBuildingSystem.FieldType GetBuildType()
    {
        return type;
    }
}
