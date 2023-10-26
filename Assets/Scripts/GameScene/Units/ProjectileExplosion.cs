using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    private int damage = 1;
    private float explosionRadius = 1f;

    private void Start() 
    {
        Explode();
        Destroy(gameObject, 0.5f);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius);

        foreach (Collider2D item in colliders)
        {
            Monster monster = item.GetComponent<Monster>();
            if(monster != null)
            {
                monster.GetComponent<Health>()?.DoDamage(damage);
            }
        }
    }
}
