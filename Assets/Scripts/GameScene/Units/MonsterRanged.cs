using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRanged : Monster
{
    [Header("Ranged")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    public new event Action OnMonsterAttacked;

    protected override void AttackTarget()
    {
        if(NPCExsitsAndAlive(target))
        {
            if(attackTimer > attackSpeed)
            {
                ShootProjectile();
                OnMonsterAttacked?.Invoke();
                attackTimer = 0;
            }
        }else{
            ChangeState(UnitState.walk);
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab,transform.position,Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(target,projectileSpeed,attackDamage);
    }
}
