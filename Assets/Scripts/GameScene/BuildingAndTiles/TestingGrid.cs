using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestingGrid : MonoBehaviour
{

    private Grid<HeatMapGridObject> grid;

    void Start()
    {
        grid = new Grid<HeatMapGridObject>(8,12,10f, new Vector3(0,30,0),(Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g,x,y));
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());
            if(heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
        }
        else if(Input.GetMouseButton(1))
        {
            Debug.Log(grid.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
    }

}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid; 
    private int x;
    private int y;
    private int value;
 
    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {  
        value += addValue;
        value = Mathf.Clamp(value,MIN,MAX);
        grid.TriggerGridObjectChanged(x,y);
    }

    public float GetValueNormalized()
    {
        return (float) value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
