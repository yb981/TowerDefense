using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Monster : MonoBehaviour
{
    public static event Action OnMonsterDied;

    enum EnemyState 
    {
        walk,
        chase,
        attack,
    }

    [SerializeField] private int rewardCreditsOnKill;
    [SerializeField] private int rewardScoreOnKill;
    [SerializeField] private float triggerRange = 50f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private int attackDamage = 1;


    private Transform[] allNPCs;

    private Transform king;
    private Transform target;

    private EnemyState state;
    private bool playing;
    private float timer;
    private float currentMovementSpeed = 10f;
    private bool isAlive = true;
    private float attackTimer;

    private Health health;

    private void Start() 
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += Health_OnHealthChanged;

        king = FindObjectOfType<King>().GetComponent<Transform>();
        state = EnemyState.walk;
        playing = true;

        GetAllNPCs();
    }

    private void Update() 
    {
        if(playing && isAlive)
        {
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

    private void AttackTarget()
    {
        if(target != null)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer > attackSpeed)
            {
                target.GetComponent<Health>().DoDamage(attackDamage);
                attackTimer = 0;
            }
        }else{
            ChangeState(EnemyState.walk);
        }
    }

    private void ChaseTarget()
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

    private void FindTargets()
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

    private void Health_OnHealthChanged()
    {
        if(health.GetHealth() <= 0)
        {
            Die();
        }
    }

    private void MoveTo(Transform goalPosition)
    {
        Vector3 dir = goalPosition.position-transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * currentMovementSpeed);
    }

    public void Die()
    {
        isAlive = false;
        OnMonsterDied?.Invoke();
        RewardPlayer();
        Destroy(gameObject);
    }

    private void RewardPlayer()
    {
        PlayerStats.Instance.AddCredits(rewardCreditsOnKill);
        UtilsClass.CreateWorldTextPopup(rewardCreditsOnKill.ToString(),transform.position,0.5f);
        
        PlayerStats.Instance.AddScore(rewardScoreOnKill);
    }

    private void GetAllNPCs()
    {
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs = new Transform[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
        {
            allNPCs[i] = npcs[i].GetComponent<Transform>();
            Debug.Log(npcs[i]);
        }
    }

    private void ChangeState(EnemyState newState)
    {
        state = newState;
        UtilsClass.CreateWorldTextPopup(state.ToString(),transform.position);
    }
}
