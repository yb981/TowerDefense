using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{

    private TileSelection tileSelection;

    private void Start() 
    {
        tileSelection = FindObjectOfType<TileSelection>();
        tileSelection.OnTileSelected += TileSelection_OnTileSelected;
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        ApplyNewBonus(e.bonusEffect);
    }

    public void ApplyNewBonus(BonusEffect bonus)
    {
        switch(bonus.GetBonusType())
        {
            case BonusType.attackSpeed: ApplyAttackSpeed(bonus); break;
            case BonusType.attackDamage: ApplyAttackDamage(bonus); break;
        }
    }

    private void ApplyAttackSpeed(BonusEffect bonus)
    {
        PlayerStats.Instance.AddBonusAttackSpeed(bonus.GetEffectValue());
    }

    private void ApplyAttackDamage(BonusEffect bonus)
    {
        PlayerStats.Instance.AddBonusAttackDamage(bonus.GetEffectValue());
    }
}
