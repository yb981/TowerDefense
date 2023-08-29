using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SubTileVisuals : MonoBehaviour
{

    [SerializeField] private TileBase[] buildingLevel;
    private Tile tile;

    private void Awake() 
    {
        tile = GetComponent<Tile>();
        tile.OnNewSubTile += Tile_OnNewSubTile;
    }

    private void Tile_OnNewSubTile(object sender, Tile.OnNewSubTileEventArgs e)
    {
        if(e.fieldType == GridBuildingSystem.FieldType.building)
        {
            e.tileTilemap.SetTile(new Vector3Int(e.pos.x % 10,e.pos.y % 10,0),buildingLevel[e.buildHeight]);
        }
    }

    public int GetNumberOfLevelHeights()
    {
        Debug.Log("returning buildingLevel Length");
        return buildingLevel.Length;
    }
}
