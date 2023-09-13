using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
   
    public static WaveManager Instance { private set; get;}

    private SpawnManager spawnManager;
    private int currentWave = 1;
    private int monstersDied = 0;
    private int monstersSpawned = 0;
    [SerializeField] private int moneyPerWave = 10;

    private void Awake() 
    {
        Instance = this;    
    }

    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        NormalMonster.OnMonsterDied += Monster_OnMonsterDied;
    }

    private void Monster_OnMonsterDied()
    {
        monstersDied++;
        if(monstersDied==monstersSpawned)
        {
            EndWave();
        }
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        spawnManager.StartSpawn();
    }

    private void EndWave()
    {
        LevelManager.instance.EndWave();
        WaveRewards();
        currentWave++;
        ResetVariables();
    }

    private void WaveRewards()
    {
        PlayerStats.Instance.AddCredits(moneyPerWave);
    }

    private void ResetVariables()
    {
        monstersDied = 0;
        monstersSpawned = 0;
    }

    public void SetMonsterSpawnAmount(int amount)
    {
        monstersSpawned = amount;
    }

    public void AddMonsterSpawnAmount(int amount)
    {
        monstersSpawned += amount;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
