using UnityEngine;
using Pathfinding;

public class EnemyMovementFloating : EnemyMovementBase
{
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;

    public float nextWaypointDistance = 0.5f;

    [Header("Floating Settings")]
    public LayerMask groundLayer;
    public float hoverHeight = 1.2f;
    public float verticalSmooth = 4f;
    public float obstacleCheckDistance = 0.6f;

    [Header("Hover Limits")]
    public float minHover = 0.5f;
    public float maxHover = 3f;

    [Header("Sensors")]
    public Transform frontSensor;

    private Transform target;

    void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public override void Initialize(Enemy owner)
    {
        base.Initialize(owner);

        rb.gravityScale = 0f;
        rb.linearDamping = 1f;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.4f);
    }

    void UpdatePath()
    {
        if (target == null) return;
        if (seeker.IsDone()) seeker.StartPath(rb.position, target.position, OnPathComplete);
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
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.1f);
            return;
        }

        Vector2 waypoint = path.vectorPath[currentWaypoint];
        Vector2 dir = (waypoint - rb.position).normalized;

        float distanceToTarget = target != null ? Vector2.Distance(rb.position, target.position) : Mathf.Infinity;

        // Hover Y padrão baseado no chão
        float groundY = GetGroundHeight();
        float targetY = groundY + hoverHeight;

        // Se player estiver próximo, trava a altura
        if (distanceToTarget < 1.2f)
        {
            targetY = rb.position.y;
        }
        else
        {
            // Raycast frontal para detectar obstáculo
            if (frontSensor != null)
            {
                Vector2 rayOrigin = frontSensor.position;
                RaycastHit2D hitFront = Physics2D.Raycast(rayOrigin, new Vector2(Mathf.Sign(dir.x), 0), obstacleCheckDistance, groundLayer);

                if (hitFront.collider != null)
                {
                    float obstacleTop = hitFront.collider.bounds.max.y;
                    targetY = Mathf.Max(targetY, obstacleTop + hoverHeight); // sobe acima do obstáculo
                }
            }

            // Raycast para detectar queda à frente
            Vector2 groundOrigin = (Vector2)frontSensor.position + new Vector2(Mathf.Sign(dir.x) * 0.1f, 0);
            RaycastHit2D hitGround = Physics2D.Raycast(groundOrigin, Vector2.down, 5f, groundLayer);

            if (hitGround.collider != null)
            {
                float groundAheadY = hitGround.point.y + hoverHeight;
                targetY = Mathf.Clamp(targetY, minHover + groundAheadY - hoverHeight, maxHover + groundAheadY - hoverHeight);
            }
        }

        // Suaviza movimento vertical
        float verticalVel = (targetY - rb.position.y) * verticalSmooth;

        // Aplica movimento
        rb.linearVelocity = new Vector2(dir.x * speed, verticalVel);

        // Próximo waypoint
        if (Vector2.Distance(rb.position, waypoint) < nextWaypointDistance)
            currentWaypoint++;
    }

    float GetGroundHeight()
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
}