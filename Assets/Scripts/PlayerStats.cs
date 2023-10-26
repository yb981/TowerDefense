using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set;}

    public static event Action OnCreditsChanged;
    public static event Action OnScoreChanged;
    public static event Action BonusChanged;

    [SerializeField] private int lives;
    [SerializeField] private int credits;
    [SerializeField] private int score;

    private float attackSpeedBonus = 0;
    private int attackDamageBonus = 0;

    private void Awake() 
    {
        if(Instance == null) Instance = this;    
    }

    public int GetCredits()
    {
        return credits;
    }

    public void SetCredits(int value)
    {
        credits = value;
        OnCreditsChanged?.Invoke();
    }

    public void AddCredits(int value)
    {
        credits += value;
        OnCreditsChanged?.Invoke();
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int value)
    {
        score += value;
        OnScoreChanged?.Invoke();
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke();
    }

    // Boni
    public void AddBonusAttackDamage(int additionalDamage)
    {
        attackDamageBonus += additionalDamage;
        BonusChanged?.Invoke();
    }

    public int GetBonusAttackDamage()
    {
        return attackDamageBonus;
    }

    public void AddBonusAttackSpeed(float additionalSpeed)
    {
        attackSpeedBonus += additionalSpeed;
        BonusChanged?.Invoke();
    }

    public float GetBonusAttackSpeed()
    {
        return attackSpeedBonus;
    }
}
