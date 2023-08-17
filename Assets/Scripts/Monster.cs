using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    public static event Action OnMonsterDied;
    public event Action OnMonsterAttacked;

    [SerializeField] private MonsterStatsSO monsterStatsSO;

    protected int rewardCreditsOnKill;
    protected int rewardScoreOnKill;
    protected float triggerRange;
    protected float attackRange;
    protected float attackSpeed;
    protected int attackDamage;
    protected float spawnTime = 0.5f;

    private Transform[] allNPCs;

    private Transform king;
    protected Transform target;

    private bool playing;
    private float timer;
    
    private bool isAlive = true;
    protected float attackTimer;

    protected override void Start() 
    {
        base.Start();

        king = FindObjectOfType<King>().GetComponent<Transform>();

        GetAllNPCs();
        SetStatsFromMonsterStatsSO();

        // Init variables
        playing = true;
        StartCoroutine(Spawn());
    }

    protected virtual void SetStatsFromMonsterStatsSO()
    {
        rewardCreditsOnKill = monsterStatsSO.rewardCreditsOnKill;
        rewardScoreOnKill = monsterStatsSO.rewardScoreOnKill;
        triggerRange = monsterStatsSO.triggerRange;
        attackRange = monsterStatsSO.attackRange;
        attackSpeed = monsterStatsSO.attackSpeed;
        attackDamage = monsterStatsSO.attackDamage;
        defaultMovementSpeed = monsterStatsSO.currentMovementSpeed;
    }

    protected virtual void Update() 
    {
        if(playing && isAlive)
        {

            // increase attack timer (so does not have to wait during attack)
            attackTimer += Time.deltaTime;

            switch(state)
            {
                case UnitState.walk:
                    MoveTo(king);
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

    protected IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);
        ChangeState(UnitState.walk);
    }

    protected virtual void AttackTarget()
    {
        if(NPCExsitsAndAlive(target))
        {
            if(attackTimer > attackSpeed)
            {
                target.GetComponent<Health>().DoDamage(attackDamage);
                OnMonsterAttacked?.Invoke();
                attackTimer = 0;
            }
        }else{
            ChangeState(UnitState.walk);
        }
    }

    protected virtual void ChaseTarget()
    {
        if(NPCExsitsAndAlive(target))
        {
            if(Vector3.Distance(transform.position,target.position) < attackRange)
            {
                ChangeState(UnitState.attack);
            }else{
                MoveTo(target);
            }
        }else{
            // if no enemy keep walk / find new enemies
            ChangeState(UnitState.walk);
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
            if(NPCExsitsAndAlive(npcTarget))
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
            ChangeState(UnitState.chase);
        }
    }

    protected bool NPCExsitsAndAlive(Transform target)
    {
        return (target != null) && (target.gameObject.activeSelf == true);
    }

    protected override void Die()
    {
        isAlive = false;
        OnMonsterDied?.Invoke();
        RewardPlayer();
        base.Die();
    }

    protected void RewardPlayer()
    {
        PlayerStats.Instance.AddCredits(rewardCreditsOnKill);
        PlayerStats.Instance.AddScore(rewardScoreOnKill);
    }

    protected void GetAllNPCs()
    {
        Minion[] npcs = FindObjectsOfType<Minion>();
        allNPCs = new Transform[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
        {
            allNPCs[i] = npcs[i].GetComponent<Transform>();
        }
    }
} 
