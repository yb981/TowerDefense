using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpMainTileEffect;
    private TileSelection tileSelection;
    private void Start()
    {
        Hide();

        tileSelection = FindObjectOfType<TileSelection>();
        tileSelection.OnTileSelected += TileSelection_OnTileSelected;

        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        Show();
        DisplayCurrentTile(e);
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        Hide();
    }

    private void DisplayCurrentTile(TileSelection.OnTileSelectedEventArgs e)
    {
        tmpName.text = e.tileBlueprint.tilePrefab.name;
        tmpMainTileEffect.text = e.tileBlueprint.mainTileEffect.ToString();
    }

    private void Show()
    {
        Debug.Log("showing Tooltip");
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        Debug.Log("hiding Tooltip");
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
    }
}
