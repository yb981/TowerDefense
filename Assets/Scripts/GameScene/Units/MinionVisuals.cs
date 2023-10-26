using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionVisuals : MonoBehaviour
{
    private static string ANIMATION_MOVING = "isMoving";
    private static string ANIMATION_ATTACKING = "isAttacking";
    private static string ANIMATION_HURT = "isHurt";
    private static string ANIMATION_NOTPLAYING = "isNotPlaying";

    private Health health;
    private Minion minion;
    private Minion.UnitState minionState;
    private Animator animator;

    void  Start()
    {
        animator = GetComponentInParent<Animator>();

        health = GetComponentInParent<Health>();
        health.OnDamageTaken += Health_OnDamageTaken;

        minion = GetComponentInParent<Minion>();
        minion.OnStateChanged += Minion_OnStateChanged;
        minion.OnAttacked += Minion_OnAttacked;

        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
    }

    private void Health_OnDamageTaken()
    {
        animator.SetTrigger(ANIMATION_HURT);
    }

    private void LevelManager_OnLevelPhasePostPlay()
    {
        animator.SetTrigger(ANIMATION_NOTPLAYING);
    }

    private void Minion_OnAttacked()
    {
        animator.SetTrigger(ANIMATION_ATTACKING);
    }

    private void Minion_OnStateChanged()
    {
        minionState = minion.GetState();

        switch(minionState)
        {
            case Minion.UnitState.idle: animator.SetBool(ANIMATION_MOVING, false); break;
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
