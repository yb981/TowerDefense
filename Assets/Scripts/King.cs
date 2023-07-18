using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Minion
{
    protected override void ResetInstance()
    {
        playing = false;
        transform.position = spawnPoint;
    }

    protected override void Die()
    {
        LevelManager.instance.GameOver();
        base.Die();
    }
}