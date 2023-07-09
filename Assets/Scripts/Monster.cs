using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Transform goal;

    private bool playing;
    private float timer;
    private float currentMovementSpeed = 10f;

    private void Start() 
    {
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;

        goal = FindObjectOfType<King>().GetComponent<Transform>();
        playing = true;
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        playing = false;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        playing = true;
    }

    private void Update() 
    {
        if(playing)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 dir = goal.position-transform.position;

        transform.Translate(dir.normalized * Time.deltaTime * currentMovementSpeed);
    }
}
