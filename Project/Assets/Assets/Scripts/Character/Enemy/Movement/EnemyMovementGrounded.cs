using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class EnemyMovementGrounded : EnemyMovementBase
{
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    public float nextWaypointDistance = 0.6f;

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

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public override void Initialize(Enemy ownerEnemy)
    {
        base.Initialize(ownerEnemy);
        InvokeRepeating(nameof(UpdatePath), 0f, 0.4f);
    }

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

    public override void OnFixedUpdate()
    {
        if (rb == null) return;

        bool grounded = Physics2D.OverlapCircle(feetPoint.position, groundedRadius, groundLayer);

        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            // no path - decelerate
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

    public override void SetTarget(Transform t)
    {
        target = t;
        // Start path immediately
        if (seeker != null && rb != null && t != null && seeker.IsDone())
            seeker.StartPath(rb.position, t.position, OnPathComplete);
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
                // small horizontal push towards waypoint
                float dirX = Mathf.Sign(nextWP.x - rb.position.x);
                v.x = dirX * Mathf.Abs(v.x);
                rb.linearVelocity = v;
                lastJumpTime = Time.time;
                currentWaypoint = lookIndex;
            }
        }
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
}
