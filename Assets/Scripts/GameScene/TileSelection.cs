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
        public Transform tilePrefab;
    }

    [SerializeField] SelectionButton[] selectionButtons;
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
        foreach (SelectionButton button in selectionButtons)
        {
            button.SetTilePrefab(RandomPrefab());
        }
    }

    private Transform RandomPrefab()
    {
        return tilePrefabs[UnityEngine.Random.Range(0,tilePrefabs.Length)];
    }

    public void Selected(Transform selectedPrefab)
    {
        OnTileSelected?.Invoke(this, new OnTileSelectedEventArgs{
            tilePrefab = selectedPrefab
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
