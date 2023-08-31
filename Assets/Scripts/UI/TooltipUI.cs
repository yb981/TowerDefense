using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpMainTileEffect;
    [SerializeField] private Transform effectDetailsBox;
    [SerializeField] private Transform effectTextPrefab;
    private TileSelection tileSelection;
    private TileEffects tileEffects;
    private bool hovering = false;
    private bool buildingTiles = false;

    private void Start()
    {
        Hide();

        tileEffects = FindObjectOfType<TileEffects>();

        tileSelection = FindObjectOfType<TileSelection>();
        tileSelection.OnTileSelected += TileSelection_OnTileSelected;

        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
    }

    private void TileSelection_OnTileSelected(object sender, TileSelection.OnTileSelectedEventArgs e)
    {
        Show();
        buildingTiles = true;
        DisplayCurrentTile(e);
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        buildingTiles = false;
        if(hovering) return;
        Hide();
    }

    private void DisplayCurrentTile(TileSelection.OnTileSelectedEventArgs e)
    {
        SetToolTipText(e.tileBlueprint.tilePrefab.GetComponent<Tile>().GetTileName(), e.tileBlueprint.mainTileEffect);
    }

    private void DisplayCurrentTile(TileGrid.GridTileObject tileGridObject)
    {
        Tile tile = tileGridObject.GetTransform().GetComponent<Tile>();
        SetToolTipText(tile.GetTileName(), tile.GetTileEffect());
    }

    private void SetToolTipText(string name, MainTileEffect mainTileEffect)
    {
        tmpName.text = name;
        tmpMainTileEffect.text = mainTileEffect.ToString();
        AddAllEffectDetails(mainTileEffect);
    }

    private void AddAllEffectDetails(MainTileEffect mainTileEffect)
    {
        Transform newText = Instantiate(effectTextPrefab,Vector3.zero,Quaternion.identity,effectDetailsBox);
        newText.GetComponent<TextMeshProUGUI>().text = tileEffects.GetMainTileEffectText(mainTileEffect);
    }

    private void RemoveAllEffectDetails()
    {
        Transform[] effectDetails = effectDetailsBox.GetComponentsInChildren<Transform>(true);

        foreach (Transform effectDetail in effectDetails)
        {
            // skip parent object
            if(effectDetail == effectDetailsBox) continue; 

            GameObject.Destroy(effectDetail.gameObject);
        }
    }

    public void NewHoverTile(TileGrid.GridTileObject currentHover)
    {
        if(currentHover.GetTransform() == null) 
        {
            hovering = false;
            if(!buildingTiles) Hide();
        }else{
            RemoveAllEffectDetails();
            hovering = true;
            Show();
            DisplayCurrentTile(currentHover);
        }
    }

    private void Show()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        RemoveAllEffectDetails();
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
    }
}
