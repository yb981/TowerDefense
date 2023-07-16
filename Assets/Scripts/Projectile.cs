using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int damage = 2;
    private Transform target;

    void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (target == null)
        {
            Debug.Log("projectile self desetroy because no target");
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * projectileSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NormalMonster monster = other.GetComponent<NormalMonster>();
        if (monster != null)
        {
            monster.GetComponent<Health>().DoDamage(damage);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}