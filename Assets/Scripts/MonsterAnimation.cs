using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private static string ANIMATION_MOVING = "isMoving";
    private static string ANIMATION_ATTACKING = "isAttacking";
    private static string ANIMATION_HURT = "isHurt";
    private static string ANIMATION_NOTPLAYING = "isNotPlaying";

    private Health health;
    private Monster monster;
    private Minion.UnitState state;
    private Animator animator;

    void  Start()
    {
        animator = GetComponentInParent<Animator>();

        health = GetComponentInParent<Health>();
        health.OnDamageTaken += Health_OnDamageTaken;

        monster = GetComponentInParent<Monster>();
        monster.OnStateChanged += Monster_OnStateChanged;
        monster.OnMonsterAttacked += Monster_OnMonsterAttacked;

        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
        Monster_OnStateChanged();
    }

    private void Health_OnDamageTaken()
    {
        animator.SetTrigger(ANIMATION_HURT);
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        animator.SetTrigger(ANIMATION_NOTPLAYING);
    }

    private void Monster_OnMonsterAttacked()
    {
        animator.SetTrigger(ANIMATION_ATTACKING);
    }

    private void Monster_OnStateChanged()
    {
        state = monster.GetState();

        switch(state)
        {
            case Minion.UnitState.spawn: animator.SetBool(ANIMATION_MOVING, false); break;
            case Minion.UnitState.idle: animator.SetBool(ANIMATION_MOVING, false); break;
            case Minion.UnitState.walk: animator.SetBool(ANIMATION_MOVING, true); break;
            case Minion.UnitState.chase: animator.SetBool(ANIMATION_MOVING, true); break;
            case Minion.UnitState.attack: animator.SetBool(ANIMATION_MOVING, false); break;
            default: break;
        }
    }

    private void OnDestroy() 
    {
        LevelManager.instance.OnLevelPhasePostPlay -= LevelManager_OnLevelPhasePostPlay;
    }
}
