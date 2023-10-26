using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnHealthChanged;
    public event Action OnDamageTaken;

    [SerializeField] private int maxHealth;
    [SerializeField] private int armor = 0;
    private int health;
    
    private void Start() 
    {
        health = maxHealth;    
        OnHealthChanged?.Invoke();
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
        OnHealthChanged?.Invoke();
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        OnHealthChanged?.Invoke();
    }

    public void DoDamage(int damage)
    {
        if(health == 0) return;

        if(health+armor - damage <= 0)
        {
            health = 0;
        }else{
            health -= Mathf.Clamp(damage-armor, 1,int.MaxValue);
        }
        OnHealthChanged?.Invoke();
        OnDamageTaken?.Invoke();
    }
}
