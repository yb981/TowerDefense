using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoss : Spawner
{
    [SerializeField] private Transform bossPrefab;

    private bool bossSpawned = false;

    protected override void SpawnManager_OnSpawnEnemies(object sender, SpawnManager.OnSpawnEnemiesEventArgs e)
    {
        if (!bossSpawned) 
        {
            SpawnEnemy(bossPrefab);
            bossSpawned = true;
        }
    }
}
