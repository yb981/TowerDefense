using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI tmpWave;
    [SerializeField] private TextMeshProUGUI tmpScore;
    [SerializeField] private TextMeshProUGUI tmpSpawns;
    [SerializeField] private Image bossIcon;
    private TileGrid tileGridComponent;
    private SpawnManager spawnManager;
    private static string CURRENT_WAVE = "Current Wave: ";
    private static string SPAWNS = " spawning";
    private static string SCORE = "Score: ";
    private int spawnNumber = 0;

    void Start()
    {
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
        PlayerStats.OnScoreChanged += PlayerStats_OnScoreChanged;

        SpawnManager.OnSpawnsUpdated += SpawnManager_OnSpawnUpdated;

        tileGridComponent = FindObjectOfType<TileGrid>();

        Invoke("UpdateAllTexts",0.5f);
    }

    private void SpawnManager_OnSpawnUpdated(object sender, EventArgs e)
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
        SpawnTextWaiting();
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
        spawnNumber = SpawnManager.Instance.GetSpawnsTotalThisWave();
        tmpSpawns.text = spawnNumber + SPAWNS;

        if(SpawnManager.Instance.GetBossSpawning())
        {
            bossIcon.enabled = true;
        }
    }

    private void SpawnTextWaiting()
    {
        tmpSpawns.text = "---";
        bossIcon.enabled = false; 
    }

    private void UpdateScoreText()
    {
        tmpScore.text = SCORE + PlayerStats.Instance.GetScore();
    }
}
