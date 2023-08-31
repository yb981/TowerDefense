using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    private Button button;
    private Transform tilePrefab;
    private TileBlueprint tileBlueprint;
    private MainTileEffect mainTileEffect;
    private TileSelection tileSelection;

    void Start()
    {
        tileSelection = GetComponentInParent<TileSelection>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Selected);
    }

    private void Selected()
    {
        tileSelection.Selected(tileBlueprint);
    }

    public void SetTilePrefab(TileBlueprint tileBlueprint)
    {
        this.tileBlueprint = tileBlueprint;
        mainTileEffect = tileBlueprint.mainTileEffect;
        tilePrefab = tileBlueprint.tilePrefab;
        SetButtonVisual();
    }

    private void SetButtonVisual()
    {
        TextMeshProUGUI buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonText.text = tilePrefab.GetComponent<Tile>().GetTileName();
    }
}
