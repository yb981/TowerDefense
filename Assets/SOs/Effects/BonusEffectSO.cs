using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BonusType
{
    attackSpeed,
    attackDamage
}

public enum Rarity
{
    normal,
    rare,
    epic,
    legendary
}

[CreateAssetMenu(fileName = "BonusEffectSO", menuName = "New BonusEffect", order = 0)]
public class BonusEffectSO : ScriptableObject {
    [SerializeField] private string bonusName;
    [SerializeField] private BonusType bonusType;
    [SerializeField] private int[] rarityValues = new int[Enum.GetValues(typeof(Rarity)).Length];

    public string GetName()
    {
        return bonusName;
    }

    public BonusType GetBonusType()
    {
        return bonusType;
    }

    public int[] GetRarityValues()
    {
        return rarityValues;
    }
}

