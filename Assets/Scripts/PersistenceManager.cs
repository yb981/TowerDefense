using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance {get; private set;}
    public string LastScene { get => lastScene; set => lastScene = value; }

    private int score;
    private int waveCount;
    private TimeSpan playTime;
    private string lastScene;

    private void Awake() 
    {
        HandleSingleton();
    }

    public void SaveScore(int value)
    {
        score = value;
    }

    public int LoadScore()
    {
        return score;
    }

    public int GetWaveCount()
    {
        return waveCount;
    }

    public void SetWaveCount(int value)
    {
        waveCount = value;
    }

    public TimeSpan GetPlayTime()
    {
        return playTime;
    }

    public void SetPlayTime(TimeSpan value)
    {
        playTime = value;
    }

    private void HandleSingleton()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
