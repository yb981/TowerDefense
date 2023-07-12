using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    
    [SerializeField] private int gridWidth = 1;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 1f;
    private Grid<GridBuildingSystem.GridObject> towerGrid;

    void Start()
    {
        towerGrid = new Grid<GridBuildingSystem.GridObject>(gridWidth, gridHeight,cellSize, transform.position,(Grid<GridBuildingSystem.GridObject> g, int x, int y) => new GridBuildingSystem.GridObject(g,x,y)); 
    }

    public Grid<GridBuildingSystem.GridObject> GetGrid()
    {
        return towerGrid;
    }
}
