using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundTrack : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start() 
    {
        SpawnerBoss.OnBossSpawned += SpawnerBoss_OnBossSpawned;
        MonsterBoss.OnBossDied += MonsterBoss_OnBossDied;
    }

    private void SpawnerBoss_OnBossSpawned()
    {
        SoundManager.Instance.PlayMusic(music);
    }

    private void MonsterBoss_OnBossDied()
    {
        SoundManager.Instance.StopMusic();
    }
}
