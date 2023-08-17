using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Unit
{

    public event Action OnAttacked;
    public new event Action OnStateChanged;


    [SerializeField] protected float triggerRange = 3f;
    [SerializeField] protected float attackSpeed = 3f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected int attackDamage = 1; 

    //protected Health health;

    protected Transform target;
    protected Vector3 spawnPoint;
    protected bool playing = false;
    private float findTimer = 0.5f;
    private float findFrequency = 0.5f;
    protected float attackTimer = 3f;

    private Vector3 disabledPosition = new Vector3(-100f,-100f,-100f);


    protected override void Start()
    {
/*         health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged; */
        base.Start();

        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;

        // Init variables
        spawnPoint = transform.position;
        playing = false;
        state = UnitState.idle;
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
                case UnitState.spawn: 
                    // wait
                    break;
                case UnitState.idle:
                    MoveTo(spawnPoint);
                    FindTargets();
                    break;
                case UnitState.chase:
                    ChaseTarget();
                    break;  
                case UnitState.attack:
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
            ChangeState(UnitState.idle);
        }
    }

    protected virtual void ChaseTarget()
    {
        if(target != null)
        {
            if(Vector3.Distance(transform.position,target.position) < attackRange)
            {
                ChangeState(UnitState.attack);
            }else{
                MoveTo(target.position);
            }
        }else{
            // if no enemy keep walk / find new enemies
            ChangeState(UnitState.idle);
        }
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
            ChangeState(UnitState.chase);
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

    protected override void Die()
    {
        // Disable Minion
        transform.position = disabledPosition;
        gameObject.SetActive(false);
    }

    protected virtual void ResetInstance()
    {
        playing = false;
        health.SetHealth(health.GetMaxHealth());
        transform.position = spawnPoint;
        ChangeState(UnitState.idle);
        gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    protected void OnDestroy() 
    {
        health.OnHealthChanged -= Health_OnHealthChanged;

        LevelManager.instance.OnLevelPhasePlay -= LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhasePostPlay -= LevelManager_OnLevelPhasePostPlay;    
    }
}
