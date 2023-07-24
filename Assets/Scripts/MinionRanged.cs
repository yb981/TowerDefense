using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionRanged : Minion
{
    public new event Action OnAttacked;

    [Header("Ranged")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;

    protected override void AttackTarget()
    {
        if(target != null)
        {
            if(attackTimer > attackSpeed)
            {
                ShootProjectile();
                attackTimer = 0;
                OnAttacked?.Invoke();
            }
        }else{
            ChangeState(NPCState.idle);
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab,transform.position,Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(target,projectileSpeed, attackDamage);
    }
}
