using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTile : Tile
{
    [SerializeField] private GameObject spawners;
    [SerializeField] private GameObject fog;
    [SerializeField] private TileSpawnManager tileSpawnManager;
    private bool firstCall = true;
    
    protected override void Awake() 
    {
        // Deactivate visuals
        spawners.SetActive(false);

        // subscribe to placement tiles, and then enable when tile connected 
        tileGrid = FindObjectOfType<TileGrid>();

        base.Awake();
    }

    private void TileGrid_OnTileBuild()
    {
        if(firstCall)
        {
            // check if any tile connected
            if(TileConnected())
            {
                fog.SetActive(false);
                spawners.SetActive(true);
                // Need to initialize here, because before there was no connection
                tileSpawnManager.InitializeSpawners();

                SpawnManager.Instance.SetBossSpawning();
                firstCall = false;
            }
            
        }

    }

    private bool TileConnected()
    {
        if(openingNodes == null) {
            Debug.Log("No opening nodes! openingNodes == null");
            return false;
        }

        foreach (TileNode node in openingNodes)
        {
            if(node == null)
            {
                Debug.Log("nodes itself are null, nodes not initialized");
                 return false;
            }
            if(node.connectionTile != null) return true;
        }

        return false;
    }

    private void OnEnable() 
    {
        tileGrid.OnTileBuilt += TileGrid_OnTileBuild;
    }

    private void OnDisable() 
    {
        tileGrid.OnTileBuilt -= TileGrid_OnTileBuild;
    }
}
