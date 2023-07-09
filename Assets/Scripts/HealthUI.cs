using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Health health;
    private Slider hpBar;

    void Start()
    {
        hpBar = GetComponent<Slider>();
        health = GetComponentInParent<Health>();

        health.OnHealthChanged += Health_OnHealthChanged;

        UpdateHpBar();
    }

    private void Health_OnHealthChanged()
    {
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        hpBar.value = health.GetHealth()/health.GetMaxHealth();
    }

}
