using UnityEngine;

public class EnemyMovementFlying : EnemyMovementBase
{
    private Transform target;

    public float arrivalDistance = 0.1f;
    public float acceleration = 10f; // suavidade da aceleração

    public override void Initialize(Enemy ownerEnemy)
    {
        base.Initialize(ownerEnemy);

        rb.gravityScale = 0;           // Inimigo voador não cai
        rb.linearDamping = 1f;         // Suaviza travagem (substitui drag)
    }

    public override void OnFixedUpdate()
    {
        if (rb == null) return;

        Vector2 desiredVel = Vector2.zero;

        if (target != null)
        {
            Vector2 dir = (target.position - transform.position);
            float dist = dir.magnitude;

            if (dist > arrivalDistance)
            {
                desiredVel = dir.normalized * speed;
            }
        }

        // Movimento suave
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVel, acceleration * Time.fixedDeltaTime);

        // Limitar velocidade máxima
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, speed);
    }

    public override void SetTarget(Transform t) => target = t;

    public override bool IsGrounded() => false;

    public override void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
}