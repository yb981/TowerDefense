using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform enemyRange;
    [SerializeField] private Transform boss;
    [SerializeField] private Spawner spawner;
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
            if(currentWave != 0 && currentWave % 10 == 0 && i == currentWave-1)
            {
                spawner.SpawnEnemy(boss.gameObject);
            }else{
                if(i != 0 && i % 3 == 0)
                {
                    spawner.SpawnEnemy(enemyRange.gameObject);
                }else{
                    spawner.SpawnEnemy(enemy.gameObject);
                }
            }
            
            yield return new WaitForSeconds(1f);
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
