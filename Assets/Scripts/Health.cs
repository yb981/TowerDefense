using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnHealthChanged;
    public event Action OnDamageTaken;

    [SerializeField] private int maxHealth;
    private int health;
    
    private void Start() 
    {
        health = maxHealth;    
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
        if(health - damage <= 0)
        {
            health = 0;
        }else{
            health -= damage;
        }
        OnHealthChanged?.Invoke();
        OnDamageTaken?.Invoke();
    }
}
