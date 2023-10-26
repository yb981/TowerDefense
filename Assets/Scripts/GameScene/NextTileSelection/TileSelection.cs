using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelection : MonoBehaviour
{

    public event EventHandler<OnTileSelectedEventArgs> OnTileSelected;
    public class OnTileSelectedEventArgs : EventArgs
    {
        public TileBlueprint tileBlueprint;
        public BonusEffect bonusEffect;
    }

    [SerializeField] TileSelectionOption[] tileSelectionOptions;
    [SerializeField] Transform child;

    [Header("All selectable tiles")]
    [SerializeField] private Transform[] tilePrefabs;

    [Header("All selectable Boni")]
    [SerializeField] private BonusEffectSO[] boniSOs;

    private void Start() 
    {
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
        Hide();
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        Show();
        SetRandomTilesForButtons();
    }

    private void SetRandomTilesForButtons()
    {
        foreach (TileSelectionOption options in tileSelectionOptions)
        {
            TileBlueprint tileBlueprint = new TileBlueprint(RandomTilePrefab(),RandomMainTileEffect());
            options.SetTilePrefab(tileBlueprint);
            options.SetBonus(RandomBonusEffect());
        }
    }

    private MainTileEffect RandomMainTileEffect()
    {
        int randomEntry = UnityEngine.Random.Range(0, Enum.GetNames(typeof(MainTileEffect)).Length);
        return (MainTileEffect) Enum.GetValues(typeof(MainTileEffect)).GetValue(randomEntry);
    }

    private Transform RandomTilePrefab()
    {
        return tilePrefabs[UnityEngine.Random.Range(0,tilePrefabs.Length)];
    }

    private BonusEffect RandomBonusEffect()
    {
        BonusEffectSO randomEffectSO = boniSOs[UnityEngine.Random.Range(0,boniSOs.Length)];
        Rarity randomRarity = (Rarity) UnityEngine.Random.Range(0,Enum.GetValues(typeof(Rarity)).Length);
        BonusEffect newBonusEffect = new BonusEffect(randomEffectSO,randomRarity);
        Debug.Log("Rarity: "+ randomRarity);
        return newBonusEffect;
    }

    public void Selected(TileBlueprint newTileBlueprint, BonusEffect newEffectBonus)
    {
        OnTileSelected?.Invoke(this, new OnTileSelectedEventArgs{
            tileBlueprint = newTileBlueprint,
            bonusEffect = newEffectBonus
        });
        Hide();
    }

    private void Show()
    {
        child.gameObject.SetActive(true);
    }

    private void Hide()
    {
        child.gameObject.SetActive(false);
    }
}