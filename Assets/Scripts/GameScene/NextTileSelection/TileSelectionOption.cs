using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TileSelectionOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainEffectTmp;
    [SerializeField] private TextMeshProUGUI bonusEffectTmp;
    [SerializeField] private TextMeshProUGUI bonusEffectAmountTmp;
    [SerializeField] private Image panelImage;
    [SerializeField] private SelectionButton button;
    [SerializeField] private Color[] rarityColors = new Color[Enum.GetValues(typeof(Rarity)).Length];

    public void SetTilePrefab(TileBlueprint tileBlueprint)
    {
        
        button.SetTilePrefab(tileBlueprint);
        mainEffectTmp.text = tileBlueprint.mainTileEffect.ToString();
    }

    public void SetBonus(BonusEffect bonus)
    {
        button.SetBonusEffect(bonus);
        bonusEffectTmp.text = bonus.GetBonusName();
        bonusEffectAmountTmp.text = "+"+bonus.GetEffectValue().ToString();
        panelImage.color = rarityColors[(int) bonus.GetRarity()];
    }
}
