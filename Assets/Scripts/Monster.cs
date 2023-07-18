using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public static event Action OnMonsterDied;
    public event Action OnMonsterAttacked;

    enum EnemyState 
    {
        walk,
        chase,
        attack,
    }

    [SerializeField] private MonsterStatsSO monsterStatsSO;

    private int rewardCreditsOnKill;
    private int rewardScoreOnKill;
    private float triggerRange;
    private float attackRange;
    private float attackSpeed;
    protected int attackDamage;
    private float currentMovementSpeed;

    private Transform[] allNPCs;

    private Transform king;
    private Transform target;

    private EnemyState state;
    private bool playing;
    private float timer;
    
    private bool isAlive = true;
    private float attackTimer;

    protected Health health;

    protected virtual void Start() 
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;

        king = FindObjectOfType<King>().GetComponent<Transform>();

        // Init variables
        state = EnemyState.walk;
        playing = true;

        GetAllNPCs();
        SetStatsFromMonsterStatsSO();
    }

    protected virtual void SetStatsFromMonsterStatsSO()
    {
        rewardCreditsOnKill = monsterStatsSO.rewardCreditsOnKill;
        rewardScoreOnKill = monsterStatsSO.rewardScoreOnKill;
        triggerRange = monsterStatsSO.triggerRange;
        attackRange = monsterStatsSO.attackRange;
        attackSpeed = monsterStatsSO.attackSpeed;
        attackDamage = monsterStatsSO.attackDamage;
        currentMovementSpeed = monsterStatsSO.currentMovementSpeed;
    }

    protected virtual void Update() 
    {
        if(playing && isAlive)
        {

            // increase attack timer (so does not have to wait during attack)
            attackTimer += Time.deltaTime;

            switch(state)
            {
                case EnemyState.walk:
                    MoveTo(king);
                    FindTargets();
                    break;
                case EnemyState.chase:
                    ChaseTarget();
                    break;  
                case EnemyState.attack:
                    AttackTarget();
                    break;

                default: break;
            }
            
        }
    }

    protected virtual void AttackTarget()
    {
        if(target != null)
        {
            if(attackTimer > attackSpeed)
            {
                target.GetComponent<Health>().DoDamage(attackDamage);
                OnMonsterAttacked?.Invoke();
                attackTimer = 0;
            }
        }else{
            ChangeState(EnemyState.walk);
        }
    }

    protected virtual void ChaseTarget()
    {
        if(target != null)
        {
            if(Vector3.Distance(transform.position,target.position) < attackRange)
            {
                ChangeState(EnemyState.attack);
            }else{
                MoveTo(target);
            }
        }else{
            // if no enemy keep walk / find new enemies
            ChangeState(EnemyState.walk);
        }
    }

    protected virtual void FindTargets()
    {
        // find closest enemy
        float closest = triggerRange;
        target = null;
        if(allNPCs == null) return;

        foreach (Transform npcTarget in allNPCs)
        {
            if(npcTarget != null)
            {
                float rangeToNPC = Vector3.Distance(npcTarget.position,transform.position);
                if(rangeToNPC < closest)
                {
                    target = npcTarget;
                    closest = rangeToNPC;
                }
            }
        }
        if(target != null)
        {
            ChangeState(EnemyState.chase);
        }
    }

    protected virtual void Health_OnHealthChanged()
    {
        if(health.GetHealth() <= 0)
        {
            Die();
        }
    }

    private void MoveTo(Transform goalPosition)
    {
        if(goalPosition == null)
            return;

        Vector3 dir = goalPosition.position-transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * currentMovementSpeed);
    }

    protected virtual void Die()
    {
        isAlive = false;
        OnMonsterDied?.Invoke();
        RewardPlayer();
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void RewardPlayer()
    {
        PlayerStats.Instance.AddCredits(rewardCreditsOnKill);
        PlayerStats.Instance.AddScore(rewardScoreOnKill);
    }

    private void GetAllNPCs()
    {
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs = new Transform[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
        {
            allNPCs[i] = npcs[i].GetComponent<Transform>();
        }
    }

    private void ChangeState(EnemyState newState)
    {
        state = newState;
    }
}
