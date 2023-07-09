using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnHealthChanged;

    [SerializeField] private int maxHealth;
    private int health;
    
    private void Start() 
    {
        health = maxHealth;    
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
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
    }

    public void DoDamage(int damage)
    {
        if(health - damage <= 0)
        {
            health = 0;
            Die();
        }else{
            health -= damage;
        }
        OnHealthChanged?.Invoke();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
