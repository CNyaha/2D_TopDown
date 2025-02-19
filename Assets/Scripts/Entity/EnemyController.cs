using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager;
    private Transform target;

    [SerializeField] private float followRange = 15f;

    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager;
        this.target = target;
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if (weaponHnadler == null || target == null)
        {
            if(!MovementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 diretion = DirectionToTarget();

        if (distance <= followRange)
        {
            lookDirection = diretion;

            if (distance < weaponHnadler.AttackRange)
            {
                int layerMaskTarget = weaponHnadler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, diretion, weaponHnadler.AttackRange * 1.5f
                    , (1 << LayerMask.NameToLayer("Level") | layerMaskTarget));

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero;
                return;
            }

        }

    }


}
