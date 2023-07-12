using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    enum NPCState 
    {
        idle,
        chase,
        attack,
    }

    private Health health;

    private Transform target;
    private Vector3 spawnPoint;
    private NPCState state;
    private bool playing = false;
    private float findTimer = 0.5f;
    private float findFrequency = 0.5f;
    private float triggerRange = 3f;
    private float attackTimer = 3f;
    private float attackSpeed = 3f;
    private float attackRange = 1f;
    private int attackDamage = 1;
    private float defaultMovementSpeed = 2f;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;

        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;

        // Init variables
        spawnPoint = transform.position;
        playing = false;
        state = NPCState.idle;
    }

    private void Update() 
    {
        if(playing)
        {
            // increase attack timer (so does not have to wait during attack)
            attackTimer += Time.deltaTime;
            findTimer += Time.deltaTime;

            switch(state)
            {
                case NPCState.idle:
                    MoveTo(spawnPoint);
                    FindTargets();
                    break;
                case NPCState.chase:
                    ChaseTarget();
                    break;  
                case NPCState.attack:
                    AttackTarget();
                    break;

                default: break;
            }
        }
    }

    private void MoveTo(Vector3 goalPosition)
    {
        Vector3 dir = goalPosition-transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * defaultMovementSpeed);
    }

    private void AttackTarget()
    {
        if(target != null)
        {
            if(attackTimer > attackSpeed)
            {
                target.GetComponent<Health>()?.DoDamage(attackDamage);
                attackTimer = 0;
            }
        }else{
            ChangeState(NPCState.idle);
        }
    }

    private void ChaseTarget()
    {
        if(target != null)
        {
            if(Vector3.Distance(transform.position,target.position) < attackRange)
            {
                ChangeState(NPCState.attack);
            }else{
                MoveTo(target.position);
            }
        }else{
            // if no enemy keep walk / find new enemies
            ChangeState(NPCState.idle);
        }
    }

    private void ChangeState(NPCState newState)
    {
        state = newState;
    }

    private void FindTargets()
    {
        if(findTimer < findFrequency)
            return;

        Monster[] monsters = FindObjectsOfType<Monster>();
        float closestDistance = triggerRange;
        foreach (Monster monster in monsters)
        {
            float rangeToEnemy = Vector3.Distance(monster.transform.position, transform.position);
            if(monster != null && rangeToEnemy < closestDistance)
            {
                target = monster.transform;
                closestDistance = rangeToEnemy;
            }
        }

        if(target != null)
        {
            ChangeState(NPCState.chase);
        }
        findTimer = 0;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        playing = true;
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        ResetInstance();
    }

    private void ResetInstance()
    {
        playing = false;
        health.SetHealth(health.GetMaxHealth());
        transform.position = spawnPoint;
    }


    private void Health_OnHealthChanged()
    {
        if(health.GetHealth() <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        health.OnHealthChanged -= Health_OnHealthChanged;

        LevelManager.instance.OnLevelPhasePlay -= LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhasePostPlay -= LevelManager_OnLevelPhasePostPlay;    
    }
}
