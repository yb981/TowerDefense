using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static event EventHandler<OnSpawnEnemiesEventArgs> OnSpawnEnemies;
    public class OnSpawnEnemiesEventArgs : EventArgs
    {
        public Transform spawnBlueprint;
    }


    [Header("Monster References")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform enemyRange;
    [SerializeField] private Transform enemyTank;
    [SerializeField] private Transform boss;

    [Header("Weight of enemies")]
    [SerializeField] private int enemyChance;
    [SerializeField] private int enemyRangeChance;
    [SerializeField] private int enemyTankChance;
    [SerializeField] private int bossWaves;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int startSpawnAmount;
    [SerializeField] private int additionalSpawnsPerWave;
    private int monstersToSpawnThisWave;

    WaveManager waveManager;

    private int monsterAlive = 0;

    private void Start()
    {
        waveManager = GetComponent<WaveManager>();
    }

    public void StartSpawn()
    {

        StartNextWave();
    }

    private void StartNextWave()
    {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        monstersToSpawnThisWave = GetSpawnsPerSpawnerThisWave();
        monsterAlive = monstersToSpawnThisWave;

        for (int i = 0; i < monstersToSpawnThisWave; i++)
        {
            OnSpawnEnemies?.Invoke(this, new OnSpawnEnemiesEventArgs()
            {
                spawnBlueprint = RandomEnemy()
            });

            yield return new WaitForSeconds(spawnTime);
        }

    }

    private Transform RandomEnemy()
    {
        int accumulated = enemyChance + enemyRangeChance + enemyTankChance;
        int enemyNr = UnityEngine.Random.Range(0, accumulated);

        // smaller as 3
        if (enemyNr < enemyChance)
        {
            return enemy.transform;
        }
        else if (enemyNr < enemyChance + enemyRangeChance)    // smaller as 3+2 (5)
        {
            return enemyRange.transform;
        }
        else
        {
            return enemyTank.transform;
        }
    }

    public int GetSpawnsPerSpawnerThisWave()
    {
        return startSpawnAmount + (waveManager.GetCurrentWave() - 1) * additionalSpawnsPerWave;
    }
}
