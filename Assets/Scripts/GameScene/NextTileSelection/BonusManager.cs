using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BonusManager : MonoBehaviour
{

    public static BonusManager Instance {private set; get;}

    [Header("All selectable Boni")]
    [SerializeField] private BonusEffectSO[] boniSOs;

    [SerializeField] private int[] rarityChances = new int[Enum.GetValues(typeof(Rarity)).Length];

    private TileSelection tileSelection;

    private void Awake() 
    {
        if(Instance != null) Debug.LogError("2 BnousManagers alive");
        Instance = this;     
    }

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
        PlayerStats.Instance.AddBonusAttackDamage((int) bonus.GetEffectValue());
    }

    private Rarity CalculateRandomRarity()
    {
        int raritySum = rarityChances.Sum();
        int random = UnityEngine.Random.Range(1,raritySum);
        
        int count = 0;
        for(int i = 0 ; i < rarityChances.Length ; i++)
        {
            count += rarityChances[i];
            if(random <= count)
            {
                return (Rarity) Enum.GetValues(typeof(Rarity)).GetValue(i);
            }
        }
        
        return Rarity.legendary;
    }

    public BonusEffect GetRandomBonusEffect()
    {
        BonusEffectSO randomEffectSO = boniSOs[UnityEngine.Random.Range(0,boniSOs.Length)];
        Rarity randomRarity = CalculateRandomRarity();
        BonusEffect randomBonusEffect = new BonusEffect(randomEffectSO,randomRarity);

        return randomBonusEffect;
    }
}
