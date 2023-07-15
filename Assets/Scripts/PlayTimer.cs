using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    private float startTime;
    private TimeSpan playTime;

    void Start()
    {
        LevelManager.instance.OnGameOver += LevelManager_OnGameOver;

        startTime = Time.time;
    }

    private void LevelManager_OnGameOver()
    {
        float span = Time.time - startTime;
        playTime = TimeSpan.FromSeconds(span);
    }

    public TimeSpan GetPlayTime()
    {
        return playTime;
    }
}
