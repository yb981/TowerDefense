using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    enum direction
    {
        North,
        South,
        West,
        East,
    }

    Grid<GridSpawnAreaObject> spawnGrid;
    [SerializeField] private int spawnGridWidth;
    [SerializeField] private int spawnGridHeight;
    [SerializeField] private int spawnGridCellSize;
    [SerializeField] private direction spawnLocation;

    private TileGrid tileGrid;
    private Queue<Vector3> waypointsFromSpawner;

    private void Start()
    {
        SpawnManager.OnSpawnEnemies += SpawnManager_OnSpawnEnemies;
        tileGrid = FindObjectOfType<TileGrid>();
    }

    private void SpawnManager_OnSpawnEnemies(object sender, SpawnManager.OnSpawnEnemiesEventArgs e)
    {
        TrySpawningEnemy(e.spawnBlueprint);
    }

    public void InstantiateSpawnerGrid()
    {
        spawnGrid = new Grid<GridSpawnAreaObject>(spawnGridWidth, spawnGridHeight, spawnGridCellSize, transform.position,
                                        (Grid<GridSpawnAreaObject> g, int w, int h) => new GridSpawnAreaObject(g, w, h));
    }

    private void TrySpawningEnemy(Transform enemy)
    {
        Tile currentTile = tileGrid.GetTileOfPosition(transform.position);
        Tile.TileNode currentNode;
        switch (spawnLocation)
        {
            case direction.North: currentNode = currentTile.GetNorthNode(); break;
            case direction.South: currentNode = currentTile.GetSouthNode(); break;
            case direction.West: currentNode = currentTile.GetWestNode(); break;
            case direction.East: currentNode = currentTile.GetEastNode(); break;
            default: currentNode = null; break;
        }
        if (currentNode.connectionTile == null && currentNode.entry) SpawnEnemy(enemy);
    }

    public void SpawnEnemy(Transform enemy)
    {
        WaveManager.Instance.AddMonsterSpawnAmount(1);
        Transform instantiatedEnemy = Instantiate(enemy, GetRandomSpawnPoint(), Quaternion.identity);
        Debug.Log("spawned enemy: "+ instantiatedEnemy);

        string waypointsString = "waypoints in spawner: ";
        Vector3[] array = waypointsFromSpawner.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            waypointsString += array[i] + ",";
        }
        Debug.Log(waypointsString);
        Queue<Vector3> waypointsCopy = new Queue<Vector3>(waypointsFromSpawner);
        instantiatedEnemy.GetComponent<Monster>().SetWaypoints(waypointsCopy);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        int randomX = Random.Range(0, spawnGridWidth);
        int randomY = Random.Range(0, spawnGridHeight);
        Vector3 spawnPoint = spawnGrid.GetCellCenter(randomX, randomY);
        return spawnPoint;
    }

    public void SetSpawnerWaypoints(Queue<Vector3> waypoints)
    {
        waypointsFromSpawner = waypoints;
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
