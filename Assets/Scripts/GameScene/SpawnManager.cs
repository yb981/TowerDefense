using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set;}
    public static event EventHandler OnSpawnsUpdated;
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
    private int activeSpawnersThisWave;
    private int activeSpawners = 0;
    private bool bossSpawning = false;

    private WaveManager waveManager;
    private TileGrid tileGridComponent;

    private void Awake() 
    {
        Instance = this;    
    }

    private void Start()
    {
        waveManager = GetComponent<WaveManager>();
        tileGridComponent = FindObjectOfType<TileGrid>();
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        GetSpawnersAndSpawnAmount();
    }

    private void GetSpawnersAndSpawnAmount()
    {
        activeSpawnersThisWave = 0;
        TileSpawnManager[] spawners = FindObjectsOfType<TileSpawnManager>();
        foreach(TileSpawnManager spawner in spawners)
        {
            spawner.UpdateActiveSpawner();
            
            activeSpawnersThisWave += spawner.GetActiveSpawners().Count;
        }
        OnSpawnsUpdated?.Invoke(this, EventArgs.Empty);
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
        int monstersToSpawnThisWave = CalculateSpawns();

        for (int i = 0; i < monstersToSpawnThisWave; i++)
        {
            OnSpawnEnemies?.Invoke(this, new OnSpawnEnemiesEventArgs()
            {
                spawnBlueprint = RandomEnemy()
            });

            yield return new WaitForSeconds(spawnTime);
        }
        bossSpawning = false;

    }

    private int CalculateSpawns()
    {
        int monstersToSpawnThisWave = GetSpawnsPerSpawnerThisWave();
        int totalSpawnsThisWave = GetSpawnsTotalThisWave();

        // Handle First Wave exception
        if(waveManager.GetCurrentWave() == 1) totalSpawnsThisWave = monstersToSpawnThisWave;



        // wavemanager needs to be informed how many monster spawn totally, to know when round over
        WaveManager.Instance.SetMonsterSpawnAmount(totalSpawnsThisWave);

        return monstersToSpawnThisWave;
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

    public int GetSpawnsTotalThisWave()
    {
        int totalSpawnsThisWave = GetSpawnsPerSpawnerThisWave() * activeSpawnersThisWave;
        // Consider Boss
        if(bossSpawning)
        {
            totalSpawnsThisWave++;
        }

        return totalSpawnsThisWave;
    }

    public void SetBossSpawning()
    {
        bossSpawning = true;
    }

    public bool GetBossSpawning()
    {
        return bossSpawning;
    }
}
