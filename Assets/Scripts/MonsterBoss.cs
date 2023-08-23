using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoss : Monster
{
    public static event Action OnBossDied;
    [SerializeField] private float timeToIncreaseRage = 3f;
    private float rageTimer;

    protected override void Start()
    {
        base.Start();
    }

   
    protected override void Update()
    {
        base.Update();

        rageTimer += Time.deltaTime;
        if(rageTimer > timeToIncreaseRage)
        {
            IncreaseDamage();
            rageTimer = 0f;
        }
    }

    private void IncreaseDamage()
    {
        attackDamage++;
    }

    protected override void Die()
    {
        Debug.Log("boss died!");
        OnBossDied?.Invoke();
        base.Die();
    }
}
