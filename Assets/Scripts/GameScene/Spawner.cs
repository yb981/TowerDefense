using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Grid<GridSpawnAreaObject> spawnGrid;
    [SerializeField] private int spawnGridWidth;
    [SerializeField] private int spawnGridHeight;
    [SerializeField] private int spawnGridCellSize;

    private void Awake() 
    {
        spawnGrid = new Grid<GridSpawnAreaObject>(spawnGridWidth,spawnGridHeight,spawnGridCellSize,transform.position, GridBuildingSystem.FieldType.unit,
                                                (Grid<GridSpawnAreaObject>g ,int w , int h) => new GridSpawnAreaObject(g,w,h));
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy,GetRandomSpawnPoint(),Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        int randomX = Random.Range(0,spawnGridWidth);
        int randomY = Random.Range(0,spawnGridHeight);
        Vector3 spawnPoint = spawnGrid.GetCellCenter(randomX,randomY);
        return spawnPoint;
    }

    public class GridSpawnAreaObject
    {

        private Grid<GridSpawnAreaObject> grid;
        private int x;
        private int y;

        public GridSpawnAreaObject(Grid<GridSpawnAreaObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
    }
}
