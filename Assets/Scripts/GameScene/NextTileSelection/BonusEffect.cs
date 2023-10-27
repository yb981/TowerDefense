using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEffect
{
    private BonusEffectSO bonusEffectSO;
    private BonusType bonusType;
    private Rarity rarity;
    private float effectValue;

    public BonusEffect(BonusEffectSO bonusEffectSO, Rarity rarity)
    {
        this.bonusEffectSO = bonusEffectSO;
        this.bonusType = bonusEffectSO.GetBonusType();
        this.rarity = rarity;
        this.effectValue = bonusEffectSO.GetRarityValues()[(int)rarity];
    }

    public string GetBonusName()
    {
        return bonusEffectSO.GetName();
    }

    public BonusType GetBonusType()
    {
        return bonusType;
    }

    public Rarity GetRarity()
    {
        return rarity;
    }

    public float GetEffectValue()
    {
        return effectValue;
    }
}
