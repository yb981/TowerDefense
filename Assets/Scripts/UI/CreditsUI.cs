using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    private TextMeshProUGUI TMPcredits;

    private String text = "Credits: ";

    private void Awake() 
    {
        TMPcredits = GetComponent<TextMeshProUGUI>();
        PlayerStats.OnCreditsChanged += PlayerStats_OnCreditsChanged;
    }

    private void Start() 
    {

        UpdateText();
    }
    
    private void PlayerStats_OnCreditsChanged()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text = "Credits: "+ PlayerStats.Instance.GetCredits();
        TMPcredits.text = text;
    }
}
