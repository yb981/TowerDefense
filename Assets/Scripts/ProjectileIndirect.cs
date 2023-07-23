using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileIndirect : Projectile
{
    private Vector3 startpoint;

    protected override void MoveToTarget()
    {
        Vector3 dir = new Vector3();
        dir = staticTargetPosition - transform.position;

        Vector3 horizontalDir = new Vector3(dir.x,0f,0f);
        Vector3 verticalDir = new Vector3();
        float totalHorizontalDistance = staticTargetPosition.x - startpoint.x;
        float horizontalDistance = staticTargetPosition.x - transform.position.x;

        float factor = horizontalDistance/totalHorizontalDistance;
        verticalDir = new Vector3(0f,dir.y + factor,0f);

        dir = horizontalDir + verticalDir;

        transform.position += dir.normalized * Time.deltaTime * projectileSpeed;

        if (Vector3.Distance(staticTargetPosition, transform.position) < 0.1f)
        {
            Die();
        }
    }

    public override void Setup(Transform newTarget)
    {
        base.Setup(newTarget);
        startpoint = transform.position;
    }

    private void OnDrawGizmosSelected()

    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(staticTargetPosition,0.1f);
    }
}
