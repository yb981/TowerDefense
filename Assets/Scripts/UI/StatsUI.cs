using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI tmpWave;
    [SerializeField] private TextMeshProUGUI tmpScore;
    [SerializeField] private TextMeshProUGUI tmpSpawns;
    private TileGrid tileGridComponent;
    private SpawnManager spawnManager;
    private static string CURRENT_WAVE = "Current Wave: ";
    private static string SPAWNS = " spawning";
    private static string SCORE = "Score: ";
    private int spawnNumber = 0;

    void Start()
    {
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
        PlayerStats.OnScoreChanged += PlayerStats_OnScoreChanged;

        spawnManager = FindObjectOfType<SpawnManager>();

        tileGridComponent = FindObjectOfType<TileGrid>();

        Invoke("UpdateAllTexts",0.5f);
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        UpdateSpawnText();
    }

    private void PlayerStats_OnScoreChanged()
    {
        UpdateScoreText();
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        UpdateWaveText();
        
    }

    private void UpdateAllTexts()
    {
        UpdateWaveText();
        UpdateScoreText();
        UpdateSpawnText();
    }

    private void UpdateWaveText()
    {
        tmpWave.text = CURRENT_WAVE + waveManager.GetCurrentWave();
    }

    private void UpdateSpawnText()
    {
        spawnNumber = spawnManager.GetSpawnsTotalThisWave();
        tmpSpawns.text = spawnNumber + SPAWNS;
    }

    private void UpdateScoreText()
    {
        tmpScore.text = SCORE + PlayerStats.Instance.GetScore();
    }
}
