using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SubTileVisuals : MonoBehaviour
{

    private TileBase[] buildingLevel;
    private TileBase[] groundTiles;
    private Tile tile;
    private SubTileVisualStorage subTileVisualStorage;

    private void Awake()
    {
        tile = GetComponent<Tile>();
        tile.OnNewSubTile += Tile_OnNewSubTile;

        subTileVisualStorage = FindObjectOfType<SubTileVisualStorage>();
    }

    private void GetTileSet()
    {
        MainTileEffect mainTileEffect = tile.GetTileEffect();
        buildingLevel = subTileVisualStorage.GetBuildingLevel(mainTileEffect);
        groundTiles = subTileVisualStorage.GetGroundVariations(mainTileEffect);
    }

    private void Tile_OnNewSubTile(object sender, Tile.OnNewSubTileEventArgs e)
    {
        if(buildingLevel == null) GetTileSet();
        if (e.fieldType == GridBuildingSystem.FieldType.building)
        {
            e.tileTilemap.SetTile(new Vector3Int(e.pos.x % 10, e.pos.y % 10, 0), buildingLevel[e.buildHeight]);
        }else if(e.fieldType == GridBuildingSystem.FieldType.unit)
        {
            e.tileTilemap.SetTile(new Vector3Int(e.pos.x % 10, e.pos.y % 10, 0), GetRandomTile(groundTiles));
        }
    }

    private TileBase GetRandomTile(TileBase[] tiles)
    {
        int randomNr = UnityEngine.Random.Range(0,tiles.Length);
        return tiles[randomNr];
    }

    public int GetNumberOfLevelHeights()
    {
        if(buildingLevel == null) GetTileSet();
        return buildingLevel.Length;
    }
}
