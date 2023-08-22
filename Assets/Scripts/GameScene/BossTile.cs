using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTile : Tile
{
    [SerializeField] private GameObject spawners;
    [SerializeField] private GameObject fog;
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
        }
    }

    private bool TileConnected()
    {
        if(openingNodes == null) return false;

        foreach (TileNode node in openingNodes)
        {
            if(node == null) return false;
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
