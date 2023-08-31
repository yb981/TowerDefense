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
    private static string CURRENT_WAVE = "Current Wave: ";
    private static string SPAWNS = "x spawn";
    private static string SCORE = "Score: ";
    

    void Start()
    {
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
        PlayerStats.OnScoreChanged += PlayerStats_OnScoreChanged;

        UpdateAllTexts();
    }

    private void PlayerStats_OnScoreChanged()
    {
        UpdateScoreText();
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        UpdateWaveText();
        UpdateSpawnText();
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
        tmpSpawns.text = waveManager.GetCurrentWave() + SPAWNS;
    }

    private void UpdateScoreText()
    {
        tmpScore.text = SCORE + PlayerStats.Instance.GetScore();
    }
}
