using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SubTileVisualStorage : MonoBehaviour
{
    [Header("Normal Tile")]
    [SerializeField] private TileBase[] buildingLevel;
    [SerializeField] private TileBase[] normalGroundVariations;
    
    [Header("Bloody Tile")]
    [SerializeField] private TileBase[] bloodyBuildingLevel;
    [SerializeField] private TileBase[] bloodyGroundVariations;

    public TileBase[] GetBuildingLevel(MainTileEffect type)
    {
        switch(type)
        {
            case MainTileEffect.none: return  buildingLevel;
            case MainTileEffect.bloody: return bloodyBuildingLevel;
        }
        return buildingLevel;
    }

    public TileBase[] GetGroundVariations(MainTileEffect type)
    {
        switch(type)
        {
            case MainTileEffect.none: return  normalGroundVariations;
            case MainTileEffect.bloody: return bloodyGroundVariations;
        }
        return normalGroundVariations;
    }
}
