using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnManager : MonoBehaviour
{
    private Spawner[] spawners;

    private void Start() 
    {
        spawners = GetComponentsInChildren<Spawner>();
    }

    public void SetSpawnerLocations()
    {
        spawners = GetComponentsInChildren<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            Debug.Log(spawner);
            spawner.InstantiateSpawnerGrid();
        }
    }
}
