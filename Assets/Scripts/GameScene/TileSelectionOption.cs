using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileSelectionOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainEffectTmp;
    [SerializeField] private SelectionButton button;

    public void SetTilePrefab(TileBlueprint tileBlueprint)
    {
        
        button.SetTilePrefab(tileBlueprint);
        mainEffectTmp.text = tileBlueprint.mainTileEffect.ToString();
    }
}
