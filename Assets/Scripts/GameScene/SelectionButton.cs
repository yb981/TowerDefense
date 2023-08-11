using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    private Button button;
    private Transform tilePrefab;
    private TileSelection tileSelection;

    void Start()
    {
        tileSelection = GetComponentInParent<TileSelection>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Selected);
    }

    private void Selected()
    {
        tileSelection.Selected(tilePrefab);
    }

    public void SetTilePrefab(Transform prefab)
    {
        tilePrefab = prefab;
        SetButtonVisual();
    }

    private void SetButtonVisual()
    {
        TextMeshProUGUI buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonText.text = tilePrefab.name;
    }
}
