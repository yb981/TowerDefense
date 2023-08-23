using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private TileGrid tileGrid;
    private void Start() 
    {
        MonsterBoss.OnBossDied += MonsterBoss_OnBossDied;
        tileGrid = FindObjectOfType<TileGrid>();
    }

    private void MonsterBoss_OnBossDied()
    {
        Debug.Log("bossManager recieved event, boss died");
        tileGrid.PlaceNewBossTile();
    }
}
