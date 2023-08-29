using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SubTileVisuals : MonoBehaviour
{

    [SerializeField] private TileBase[] buildingLevel;

    private GridBuildingSystem gridBuildingSystem;
    private void Start() 
    {
        GridBuildingSystem.OnNewSubTile += GridBuildingSystem_OnNewSubTile;
    }

    private void GridBuildingSystem_OnNewSubTile(object sender, GridBuildingSystem.OnNewSubTileEventArgs e)
    {
        if(e.fieldType == GridBuildingSystem.FieldType.building)
        {
            e.tileTilemap.SetTile(new Vector3Int(e.pos.x % 10,e.pos.y % 10,0),buildingLevel[e.buildHeight]);
        }
    }
}
