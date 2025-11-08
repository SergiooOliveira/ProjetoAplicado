using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Seeker))]
public class EnemyMovementFloating : EnemyMovementBase
{
    #region Fields

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    public float nextWaypointDistance = 0.6f;

    [Header("Patrol (no target)")]
    public float patrolRange = 3f;
    public float patrolSpeed = 2f;
    public float patrolWaitTime = 1.5f;

    private Vector2 patrolCenter;
    private Vector2 patrolTarget;
    private bool waiting = false;
    private bool initializedPatrol = false;

    private bool playerInSight = false;
    private bool isAttacking = false;

    [Header("Floating Settings")]
    public LayerMask groundLayer;
    public float hoverHeight = 1.2f;
    public float verticalSmooth = 4f;
    public float obstacleCheckDistance = 0.6f;
    public float minHover = 0.5f;
    public float maxHover = 3f;

    [Header("Sensors")]
    public Transform frontSensor;
    public Transform limitPoint;

    private Transform target;
    private bool readyToMove = false;

    #endregion

    #region Initialize

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public override void Initialize(Enemy owner)
    {
        base.Initialize(owner);

        rb.gravityScale = 0f;
        rb.linearDamping = 1f;

        if (limitPoint != null)
            StartCoroutine(WaitForLimitPoint());
        else
            InvokeRepeating(nameof(UpdatePath), 0f, 0.4f);
    }

    private IEnumerator WaitForLimitPoint()
    {
        // Maintain original gravity to fall to the ground
        rb.gravityScale = 1f;

        // Wait until the LimitPoint touches the ground
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(limitPoint.position, Vector2.down, 0.1f, groundLayer);
            if (hit.collider != null)
            {
                // Now that it's played, disable gravity and start patrol
                rb.gravityScale = 0f;

                readyToMove = true;
                patrolCenter = transform.position;
                ChooseNewPatrolPoint();
                initializedPatrol = true;

                break;
            }
            yield return null;
        }

        // Start updating pathfinding
        InvokeRepeating(nameof(UpdatePath), 0f, 0.4f);
    }

    private void ChooseNewPatrolPoint()
    {
        float randomX = patrolCenter.x + Random.Range(-patrolRange, patrolRange);
        patrolTarget = new Vector2(randomX, patrolCenter.y);
    }

    #endregion

    #region Pathfinding

    void UpdatePath()
    {
        if (!readyToMove) return;

        if (seeker == null || rb == null) return;
        if (target == null)
        {
            path = null;
            return;
        }

        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    #endregion

    #region Movement

    public override void OnFixedUpdate()
    {
        if (rb == null) return;

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // No target -> patrol
        if (!playerInSight && target == null)
        {
            HandlePatrol();
            return;
        }

        // Follow path (A*)
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f);
            return;
        }

        Vector2 waypoint = path.vectorPath[currentWaypoint];
        Vector2 dir = (waypoint - rb.position).normalized;

        // Vertical Hover
        float groundY = GetGroundHeight();
        float targetY = groundY + hoverHeight;

        // Obstacle ahead
        if (frontSensor != null)
        {
            Vector2 rayOrigin = frontSensor.position;
            RaycastHit2D hitFront = Physics2D.Raycast(rayOrigin, new Vector2(Mathf.Sign(dir.x), 0), obstacleCheckDistance, groundLayer);

            if (hitFront.collider != null)
            {
                float obstacleTop = hitFront.collider.bounds.max.y;
                targetY = Mathf.Max(targetY, obstacleTop + hoverHeight * 0.8f);
            }
        }

        // Hover adjusted within limits
        targetY = Mathf.Clamp(targetY, groundY + minHover, groundY + maxHover);

        float verticalVel = (targetY - rb.position.y) * verticalSmooth;

        // Final movement
        rb.linearVelocity = new Vector2(dir.x * speed, verticalVel);

        // Advance waypoint
        if (Vector2.Distance(rb.position, waypoint) < nextWaypointDistance)
            currentWaypoint++;
    }

    #endregion

    #region Patrol

    private void HandlePatrol()
    {
        if (!initializedPatrol) return;

        float distToTarget = Vector2.Distance(rb.position, patrolTarget);

        if (distToTarget < 0.2f)
        {
            if (!waiting)
                StartCoroutine(WaitAndChooseNewPatrol());
            return;
        }

        Vector2 dir = (patrolTarget - rb.position).normalized;

        float groundY = GetGroundHeight();
        float targetY = groundY + hoverHeight;
        float verticalVel = (targetY - rb.position.y) * verticalSmooth;

        rb.linearVelocity = new Vector2(dir.x * patrolSpeed, verticalVel);

        if (dir.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dir.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

    #region Helpers / Overrides

    private float GetGroundHeight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
        return hit.collider ? hit.point.y : transform.position.y;
    }

    public override void SetTarget(Transform t)
    {
        target = t;
        if (seeker != null && rb != null && t != null && seeker.IsDone())
            seeker.StartPath(rb.position, t.position, OnPathComplete);
    }

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

        // Jump check point (Front Sensor)
        if (frontSensor != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(frontSensor.position,
                frontSensor.position + Vector3.right * Mathf.Sign(transform.localScale.x) * obstacleCheckDistance);
        }

        // Limit Point
        if (limitPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(limitPoint.position, 0.12f);
        }
    }

    #endregion
}