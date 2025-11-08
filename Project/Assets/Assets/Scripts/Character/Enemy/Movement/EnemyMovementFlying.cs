using System.Collections;
using UnityEngine;

public class EnemyMovementFlying : EnemyMovementBase
{
    #region Fields

    private Transform target;

    [Header("Flying movement")]
    public float arrivalDistance = 0.1f;
    public float acceleration = 10f;

    [Header("Patrol (no target)")]
    public float patrolRange = 3f;
    public float patrolSpeed = 3.5f;
    public float patrolWaitTime = 1.5f;

    private Vector2 patrolCenter;
    private Vector2 patrolTarget;
    private bool initializedPatrol = false;
    private bool waiting = false;

    private bool playerInSight = false;
    private bool isAttacking = false;

    #endregion

    #region Initialization

    public override void Initialize(Enemy ownerEnemy)
    {
        base.Initialize(ownerEnemy);

        rb.gravityScale = 0f;       // Does not fall
        rb.linearDamping = 1f;      // Softens braking

        initializedPatrol = false;

        // Wait until defining the central patrol point
        StartCoroutine(WaitUntilReady());
    }

    private IEnumerator WaitUntilReady()
    {
        // Wait 1 frame to guarantee initial position
        yield return null;

        patrolCenter = rb.position;
        ChooseNewPatrolPoint();
        initializedPatrol = true;
    }

    private void ChooseNewPatrolPoint()
    {
        float randomX = patrolCenter.x + Random.Range(-patrolRange, patrolRange);
        float randomY = patrolCenter.y + Random.Range(-patrolRange, patrolRange);
        patrolTarget = new Vector2(randomX, randomY);
    }

    #endregion

    #region FixedUpdate / Movement

    public override void OnFixedUpdate()
    {
        if (rb == null) return;

        Vector2 desiredVel = Vector2.zero;

        if (target != null)
        {
            Vector2 dir = (Vector2)target.position - rb.position;
            float dist = dir.magnitude;

            if (dist > arrivalDistance)
                desiredVel = dir.normalized * speed;
        }
        else if (initializedPatrol && !isAttacking && !playerInSight)
        {
            HandlePatrol();
        }

        // Smooth movement
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVel, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, speed);
    }

    #endregion

    #region Patrol

    private void HandlePatrol()
    {
        float distToTarget = Vector2.Distance(rb.position, patrolTarget);

        if (distToTarget < 0.1f)
        {
            if (!waiting)
                StartCoroutine(WaitAndChooseNewPatrol());
            return;
        }

        Vector2 dir = (patrolTarget - rb.position).normalized;
        rb.linearVelocity = dir * patrolSpeed;
    }

    private IEnumerator WaitAndChooseNewPatrol()
    {
        waiting = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(patrolWaitTime);
        ChooseNewPatrolPoint();
        waiting = false;
    }

    #endregion

    #region Overrides / Utility

    public override void SetTarget(Transform t) => target = t;

    public override bool IsGrounded() => false;

    public override void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public override void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    public override void SetPlayerInSight(bool inSight)
    {
        playerInSight = inSight;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        // Patrol Center
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(patrolCenter, 0.2f);

        // Patrol area
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f); // Translucent yellow
        Gizmos.DrawSphere(patrolCenter, patrolRange);

        // Ponto de patrulha atual
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(patrolTarget, 0.15f);

        // Line from enemy to target (optional)
        if (rb != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(rb.position, patrolTarget);
        }
    }

    #endregion
}