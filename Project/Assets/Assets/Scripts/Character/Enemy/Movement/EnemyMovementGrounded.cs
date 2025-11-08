using Pathfinding;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class EnemyMovementGrounded : EnemyMovementBase
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
    private Vector2? fixedTargetPoint = null;

    private bool playerInSight = false;
    private bool isAttacking = false;

    [Header("Jump settings")]
    public Transform feetPoint;
    public LayerMask groundLayer;
    public float groundedRadius = 0.12f;
    public float minStepHeight = 0.15f;
    public float maxJumpHeight = 2f;
    public float jumpVelocity = 8f;
    public float jumpCooldown = 1.2f;
    private float lastJumpTime = -999f;

    private Transform target;

    #endregion

    #region Initialize

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public override void Initialize(Enemy ownerEnemy)
    {
        base.Initialize(ownerEnemy);

        // We haven't defined patrolCenter yet
        initializedPatrol = false;

        // Check Update/FixedUpdate to see if it is on the ground
        StartCoroutine(WaitUntilGrounded());
    }

    private IEnumerator WaitUntilGrounded()
    {
        while (!IsGrounded())
            yield return null;

        patrolCenter = transform.position;
        ChooseNewPatrolPoint();
        initializedPatrol = true;

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
        if (seeker == null || rb == null) return;
        if (target == null)
        {
            // If no target, you could compute path to patrol waypoints or stop requesting paths
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

    #region FixedUpdate / Movement

    public override void OnFixedUpdate()
    {
        if (rb == null) return;

        bool grounded = Physics2D.OverlapCircle(feetPoint.position, groundedRadius, groundLayer);

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        Vector2 destination = fixedTargetPoint ?? (target != null ? (Vector2)target.position : rb.position);

        if (!playerInSight && target == null)
        {
            HandlePatrol();
            return;
        }

        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            // No path - decelerate
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f);
            return;
        }

        // Try jump logic
        TryJumpAlongPath(grounded);

        Vector2 nextWP = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 dir = (nextWP - rb.position).normalized;
        Vector2 desiredVel = new Vector2(dir.x * speed, rb.linearVelocity.y);

        // Apply horizontal velocity directly
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVel, speed * Time.fixedDeltaTime * 10f);

        float dist = Vector2.Distance(rb.position, nextWP);
        if (dist < nextWaypointDistance) currentWaypoint++;
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
        TryJumpPatrolObstacle(dir);
        rb.linearVelocity = new Vector2(dir.x * patrolSpeed, rb.linearVelocity.y);

        if (dir.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dir.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator WaitAndChooseNewPatrol()
    {
        waiting = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        yield return new WaitForSeconds(patrolWaitTime);
        ChooseNewPatrolPoint();
        waiting = false;
    }

    #endregion

    #region Jump Logic

    private void TryJumpPatrolObstacle(Vector2 dir)
    {
        bool grounded = IsGrounded();

        if (!grounded) return;

        // Raycast origin: at foot height
        Vector2 origin = rb.position + Vector2.up * -0.05f;

        // Pure horizontal direction, ignoring y
        Vector2 horizontalDir = new Vector2(Mathf.Sign(dir.x), 0f);

        // Raycast length, adjusts according to obstacle
        float distance = 1f;

        // Draw for debug
        Debug.DrawRay(origin, horizontalDir * distance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(origin, horizontalDir, distance, groundLayer);

        if (hit.collider != null)
        {
            // Ceiling check, keep it higher than the obstacle
            Vector2 ceilingCheck = rb.position + Vector2.up * 1f;
            Collider2D overlap = Physics2D.OverlapBox(ceilingCheck, new Vector2(0.3f, 0.5f), 0f, groundLayer);

            if (overlap == null)
            {
                Vector2 v = rb.linearVelocity;
                v.y = jumpVelocity;
                rb.linearVelocity = v;
                lastJumpTime = Time.time;
            }
        }
    }

    private void TryJumpAlongPath(bool grounded)
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;
        if (!grounded) return;

        // Don't jump if too close to the player (avoid climbing on them)
        if (target != null && Vector2.Distance(rb.position, target.position) < 1.2f)
            return;

        // Look ahead for a waypoint with vertical difference
        int lookIndex = currentWaypoint;
        Vector2 nextWP = path.vectorPath[lookIndex];

        for (int i = currentWaypoint; i < Mathf.Min(path.vectorPath.Count, currentWaypoint + 4); i++)
        {
            float dy = path.vectorPath[i].y - rb.position.y;
            if (Mathf.Abs(dy) > 0.01f)
            {
                nextWP = path.vectorPath[i];
                lookIndex = i;
                break;
            }
        }

        float heightDiff = nextWP.y - rb.position.y;
        if (heightDiff >= minStepHeight && heightDiff <= maxJumpHeight)
        {
            // Check space above
            Vector2 center = new Vector2(nextWP.x, nextWP.y + 0.1f);
            Collider2D overlap = Physics2D.OverlapBox(center, new Vector2(0.3f, 0.2f), 0f, groundLayer);

            // Don't jump if the thing in front is the Player
            if (overlap != null && overlap.CompareTag("Player"))
                return;

            if (overlap == null && Time.time - lastJumpTime >= jumpCooldown)
            {
                Vector2 v = rb.linearVelocity;
                v.y = jumpVelocity;
                // Small horizontal push towards waypoint
                float dirX = Mathf.Sign(nextWP.x - rb.position.x);
                v.x = dirX * Mathf.Abs(v.x);
                rb.linearVelocity = v;
                lastJumpTime = Time.time;
                currentWaypoint = lookIndex;
            }
        }
    }

    #endregion

    #region Overrides / Utility

    public override void SetTarget(Transform t)
    {
        target = t;
        // Start path immediately
        if (seeker != null && rb != null && t != null && seeker.IsDone())
            seeker.StartPath(rb.position, t.position, OnPathComplete);
    }

    public override bool IsGrounded()
    {
        if (feetPoint == null) return false;
        return Physics2D.OverlapCircle(feetPoint.position, groundedRadius, groundLayer);
    }

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

        // Jump check point (feet)
        if (feetPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(feetPoint.position, groundedRadius);
        }
    }

    #endregion
}
