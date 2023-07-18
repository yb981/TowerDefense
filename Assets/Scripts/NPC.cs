using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public event Action OnAttacked;

    protected enum NPCState 
    {
        idle,
        chase,
        attack,
    }

    private Health health;

    protected Transform target;
    protected Vector3 spawnPoint;
    protected NPCState state;
    protected bool playing = false;
    protected float findTimer = 0.5f;
    protected float findFrequency = 0.5f;
    protected float triggerRange = 3f;
    protected float attackTimer = 3f;
    protected float attackSpeed = 3f;
    protected float attackRange = 1f;
    protected int attackDamage = 1;
    protected float defaultMovementSpeed = 2f;

    protected virtual void Start()
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

    protected virtual void Update() 
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

    protected void MoveTo(Vector3 goalPosition)
    {
        Vector3 dir = goalPosition-transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * defaultMovementSpeed);
    }

    protected virtual void AttackTarget()
    {
        if(target != null)
        {
            if(attackTimer > attackSpeed)
            {
                target.GetComponent<Health>()?.DoDamage(attackDamage);
                attackTimer = 0;
                OnAttacked?.Invoke();
            }
        }else{
            ChangeState(NPCState.idle);
        }
    }

    protected virtual void ChaseTarget()
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

    protected void ChangeState(NPCState newState)
    {
        state = newState;
    }

    protected virtual void FindTargets()
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

    protected void LevelManager_OnLevelPhasePlay()
    {
        playing = true;
    }

    protected void LevelManager_OnLevelPhasePostPlay()
    {
        ResetInstance();
    }

    protected virtual void ResetInstance()
    {
        playing = false;
        health.SetHealth(health.GetMaxHealth());
        transform.position = spawnPoint;
    }

    protected void Health_OnHealthChanged()
    {
        if(health.GetHealth() <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void OnDestroy() 
    {
        health.OnHealthChanged -= Health_OnHealthChanged;

        LevelManager.instance.OnLevelPhasePlay -= LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhasePostPlay -= LevelManager_OnLevelPhasePostPlay;    
    }
}
