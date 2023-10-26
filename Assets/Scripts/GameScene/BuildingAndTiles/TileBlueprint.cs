using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlueprint
{
    public Transform tilePrefab;
    public MainTileEffect mainTileEffect;
    public TileBlueprint(Transform tilePrefab, MainTileEffect mainTileEffect)
    {
        this.tilePrefab = tilePrefab;
        this.mainTileEffect = mainTileEffect;
    }
}
