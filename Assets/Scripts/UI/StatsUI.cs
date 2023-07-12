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
    private static string CURRENT_WAVE = "Current Wave: ";
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
    }

    private void UpdateAllTexts()
    {
        UpdateWaveText();
        UpdateScoreText();
    }

    private void UpdateWaveText()
    {
        tmpWave.text = CURRENT_WAVE + waveManager.GetCurrentWave();
    }

    private void UpdateScoreText()
    {
        tmpScore.text = SCORE + PlayerStats.Instance.GetScore();
    }
}
