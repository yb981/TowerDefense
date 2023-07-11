using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void Health_OnHealthChanged()
    {
        Die();
    }

    private void Die(){
        Destroy(gameObject);
    }
}
