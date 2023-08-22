using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnManager : MonoBehaviour
{
    private Spawner[] spawners;
    private TileGrid tileGrid;
    private WaypointAlgorithm waypointAlgorithm;
    private King king;

    private void Start() 
    {
        spawners = GetComponentsInChildren<Spawner>();
        tileGrid = FindObjectOfType<TileGrid>();
        waypointAlgorithm = new WaypointAlgorithm(tileGrid.GetTileGrid());      
        king = FindObjectOfType<King>();        
    }

    public void InitializeSpawners()
    {
        Stack<Vector3> waypoints = waypointAlgorithm.GetBestWaypointPath(transform.position,king.transform.position);
        //Debug.Log("waypoints: "+ waypoints);

        spawners = GetComponentsInChildren<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            spawner.InstantiateSpawnerGrid();

            spawner.SetSpawnerWaypoints(waypoints);
        }
    }
}
