using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private float range = 40f;
    [SerializeField] private float shootingSpeed = 1f;
    private Transform target;
    private bool playing;
    private float shootTimer = 0f;

    private void Start() 
    {
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;    
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;    
        playing = false;

        shootTimer = shootingSpeed;
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        playing = false;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        playing = true;
    }

    void Update()
    {
        if(playing){
            FindEnemies();
            Shoot();
        }
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;
        Mathf.Clamp(shootTimer, 0, shootingSpeed);
        if(target != null)
        {
            if(shootTimer >= shootingSpeed)
            {
                CreateProjectile();
                shootTimer = 0;
            }
        }
    }

    private void CreateProjectile()
    {
        Transform projectile = Instantiate(projectilePrefab,transform.position,Quaternion.identity);
        projectile.GetComponent<Projectile>().SetTarget(target);
    }

    private void FindEnemies()
    {
        Monster[] enemies = FindObjectsOfType<Monster>();

        if(enemies.Length == 0)
        {
            target = null;
            return;
        }

        float lowestDistance = range;
        foreach (Monster enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.GetComponent<Transform>().position, transform.position);
            if(distance < lowestDistance)
            {
                target = enemy.GetComponent<Transform>();
            }
        }
    }

    public float GetRange()
    {
        return range;
    }
}
