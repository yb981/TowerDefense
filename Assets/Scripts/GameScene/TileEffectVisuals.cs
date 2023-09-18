using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffectVisuals : MonoBehaviour
{
    private TileEffects tileEffects;
    private void Start() 
    {
        tileEffects = FindObjectOfType<TileEffects>();
        tileEffects.OnNewTileBonus += TileEffects_OnNewTileBonus;
    }

    private void TileEffects_OnNewTileBonus(object sender, TileEffects.OnNewTileBonusEventArgs e)
    {
        TextSpawnerUI.Instance.CreateWorldPopUpText(e.bonusType + " +" + e.bonusAmount, e.bonusObject.position);
    }
}
