using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionVisuals : MonoBehaviour
{
    private static string ANIMATION_MOVING = "isMoving";
    private static string ANIMATION_ATTACKING = "isAttacking";
    private static string ANIMATION_NOTPLAYING = "isNotPlaying";

    private Minion minion;
    private Minion.NPCState minionState;
    private Animator animator;

    void  Start()
    {
        minion = GetComponentInParent<Minion>();
        animator = GetComponentInParent<Animator>();
        minion.OnStateChanged += Minion_OnStateChanged;
        minion.OnAttacked += Minion_OnAttacked;
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;
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
            case Minion.NPCState.idle: animator.SetBool(ANIMATION_MOVING, false); break;
            case Minion.NPCState.chase: animator.SetBool(ANIMATION_MOVING, true); break;
            case Minion.NPCState.attack: animator.SetBool(ANIMATION_MOVING, false); break;
            default: break;
        }
    }
}
