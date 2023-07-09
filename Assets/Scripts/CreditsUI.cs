using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    private TextMeshProUGUI TMPcredits;
    private GridBuildingSystem gridBuildingSystem;

    private String text = "Credits: ";

    private void Awake() 
    {
        TMPcredits = GetComponent<TextMeshProUGUI>();
    }

    private void Start() 
    {
        gridBuildingSystem = FindObjectOfType<GridBuildingSystem>();
        gridBuildingSystem.OnBuilt += GridBuildingSystem_OnBuilt;
        UpdateText();
    }

    private void GridBuildingSystem_OnBuilt()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text = "Credits: "+ gridBuildingSystem.GetCredits();
        TMPcredits.text = text;
    }
}
