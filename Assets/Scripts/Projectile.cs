using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected int damage = 2;
    [SerializeField] protected bool explosion;
    protected Transform target;
    protected Vector3 staticTargetPosition;

    [Header("Coding")]
    [SerializeField] protected GameObject explosionObject;

    private void Start() 
    {
        LevelManager.instance.OnLevelPhasePostPlay += LevelManager_OnLevelPhasePostPlay;    
    }

    protected virtual void Update()
    {
        MoveToTarget();
    }
    
    private void LevelManager_OnLevelPhasePostPlay()
    {
        // If gamephase is over, instantly destroy the projectile without effect
        Destroy(gameObject);
    }

    protected virtual void SetAngle(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    protected virtual void MoveToTarget()
    {
        Vector3 dir = new Vector3();

        if (TargetStillAlive())
        {
            dir = target.position - transform.position;
        }

        transform.position += dir.normalized * Time.deltaTime * projectileSpeed;
    }

    private bool TargetStillAlive()
    {
        if (target == null)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }

    public virtual void Setup(Transform newTarget)
    {
        target = newTarget;
        staticTargetPosition = newTarget.position;

        SetAngle(newTarget.position);
    }

    public virtual void Setup(Transform newTarget, float projectileSpeed, int projectileDamage)
    {
        target = newTarget;
        staticTargetPosition = newTarget.position;

        this.projectileSpeed = projectileSpeed;
        damage = projectileDamage;

        SetAngle(newTarget.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DirectHitControl(other);
    }

    private void DirectHitControl(Collider2D other)
    {
        //Monster monster = other.GetComponent<Monster>();
        GameObject gameObjectHit = other.gameObject;
        if (gameObjectHit != null && other.transform == target)
        {
            gameObjectHit.GetComponent<Health>().DoDamage(damage);
            Die();
        }
    }

    protected virtual void Die()
    {
        if (explosion)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnDestroy() 
    {
        LevelManager.instance.OnLevelPhasePostPlay -= LevelManager_OnLevelPhasePostPlay;   
    }
}