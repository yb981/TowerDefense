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
    }

    [SerializeField] TileSelectionOption[] tileSelectionOptions;
    [SerializeField] Transform child;

    [Header("All tiles")]
    [SerializeField] private Transform[] tilePrefabs;

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
            TileBlueprint tileBlueprint = new TileBlueprint(RandomPrefab(),RandomMainTileEffect());
            options.SetTilePrefab(tileBlueprint);
        }
    }

    private MainTileEffect RandomMainTileEffect()
    {
        int randomEntry = UnityEngine.Random.Range(0, Enum.GetNames(typeof(MainTileEffect)).Length);
        return (MainTileEffect) Enum.GetValues(typeof(MainTileEffect)).GetValue(randomEntry);
    }

    private Transform RandomPrefab()
    {
        return tilePrefabs[UnityEngine.Random.Range(0,tilePrefabs.Length)];
    }

    public void Selected(TileBlueprint newTileBlueprint)
    {
        OnTileSelected?.Invoke(this, new OnTileSelectedEventArgs{
            tileBlueprint = newTileBlueprint
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
