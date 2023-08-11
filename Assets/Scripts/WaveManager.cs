using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform enemyRange;
    [SerializeField] private Transform enemyTank;
    [SerializeField] private Transform boss;
    [SerializeField] private Spawner spawner;

    [Header("Weight of enemies")]
    [SerializeField] private int enemyChance;
    [SerializeField] private int enemyRangeChance;
    [SerializeField] private int enemyTankChance;
    [SerializeField] private int bossWaves;
    [SerializeField] private float spawnTime = 1f;
    private int currentWave = 1;
    private int monsterAlive = 0;


    void Start()
    {
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        NormalMonster.OnMonsterDied += Monster_OnMonsterDied;
    }

    private void Monster_OnMonsterDied()
    {
        monsterAlive--;
        if(monsterAlive==0)
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
        monsterAlive = currentWave;
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()
    {

        for (int i = 0; i < currentWave; i++)
        {
            if(currentWave != 0 && currentWave % bossWaves == 0 && i == currentWave-1)
            {
                spawner.SpawnEnemy(boss.gameObject);
            }else{
                spawner.SpawnEnemy(RandomEnemy());
            }
            
            yield return new WaitForSeconds(spawnTime);
        }
        
    }

    private GameObject RandomEnemy()
    {
        int accumulated = enemyChance + enemyRangeChance + enemyTankChance;
        int enemyNr = UnityEngine.Random.Range(0,accumulated);

        // smaller as 3
        if(enemyNr <enemyChance)
        {
            return enemy.gameObject;
        }else if(enemyNr < enemyChance+enemyRangeChance)    // smaller as 3+2 (5)
        {
            return enemyRange.gameObject;
        }else{
            return enemyTank.gameObject;
        }
    }

    private void EndWave()
    {
        LevelManager.instance.EndWave();
        currentWave++;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
