using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnManager : MonoBehaviour
{
    private Spawner[] spawners;
    private TileGrid tileGrid;
    private WaypointAlgorithm waypointAlgorithm;
    private King king;

    private void Awake()
    {
        king = FindObjectOfType<King>();
        spawners = GetComponentsInChildren<Spawner>();
        tileGrid = FindObjectOfType<TileGrid>();
    }

    public void InitializeSpawners()
    {
        waypointAlgorithm = new WaypointAlgorithm(tileGrid.GetTileGrid());
        Stack<Vector3> waypoints = waypointAlgorithm.GetBestWaypointPath(transform.position, king.transform.position);

        spawners = GetComponentsInChildren<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            spawner.InstantiateSpawnerGrid();

            spawner.SetSpawnerWaypoints(waypoints);
        }
    }
}
