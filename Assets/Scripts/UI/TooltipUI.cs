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
        AddAllEffectDetails(e.tileBlueprint);
    }

    private void AddAllEffectDetails(TileBlueprint tileBlueprint)
    {
        Transform newText = Instantiate(effectTextPrefab,Vector3.zero,Quaternion.identity,effectDetailsBox);
        newText.GetComponent<TextMeshProUGUI>().text = tileEffects.GetMainTileEffectText(tileBlueprint.mainTileEffect);
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
