using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set;}

    public static event Action OnCreditsChanged;
    public static event Action OnScoreChanged;

    [SerializeField] private int lives;
    [SerializeField] private int credits;
    [SerializeField] private int score;

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
}
