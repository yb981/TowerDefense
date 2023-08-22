using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTile : Tile
{
    [SerializeField] private GameObject spawners;
    [SerializeField] private GameObject fog;
    [SerializeField] private TileSpawnManager tileSpawnManager;
    private void Awake() 
    {
        // Deactivate visuals
        spawners.SetActive(false);

        // subscribe to placement tiles, and then enable when tile connected 
        tileGrid = FindObjectOfType<TileGrid>();
    }

    private void TileGrid_OnTileBuild()
    {
        // check if any tile connected
        if(TileConnected())
        {
            fog.SetActive(false);
            spawners.SetActive(true);
            // Need to initialize here, because before there was no connection
            tileSpawnManager.InitializeSpawners();
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

        Debug.Log("no nodes connected to tile");
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
