using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform spawnPoint;
    private int currentWave = 1;

    void Start()
    {
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        for (int i = 0; i < currentWave; i++)
        {
            StartCoroutine(SpawnMonster());
        }
    }

    private IEnumerator SpawnMonster()
    {
        Instantiate(enemy,spawnPoint.position,Quaternion.identity);
        yield return new WaitForSeconds(1f);
    }
}
