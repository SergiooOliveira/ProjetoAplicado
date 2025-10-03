using UnityEngine;

public class HomingMissile : SpellProjectile
{
    private Transform target;

    // How fast it curves
    // [SerializeField] private float initialTurnSpeed = 100f;
    [SerializeField] private float turnSpeed = 200f;

    // Max distance to find enemies
    [SerializeField] private float targetSearchRadious = 10f;

    public override void Initialize(ProjectileSpell spell, Vector2 initialDir)
    {
        base.Initialize(spell, initialDir);
        // Pick nearest enemy
        target = FindNearestEnemy();
    }

    // TODO: Make so the spell has a slower speed in the begining and goes faster once x seconds pass
    private void FixedUpdate()
    {
        if (rb == null || target  == null) return;

        // Direction missile should move toward
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // Current facing direction
        Vector2 currentDir = rb.linearVelocity.normalized;

        // Angle difference in degrees
        float angle = Vector2.SignedAngle(currentDir, direction);

        // Clamp rotation per frame by turnSpeed
        float rotateStep = Mathf.Clamp(angle, -turnSpeed * Time.fixedDeltaTime, turnSpeed * Time.fixedDeltaTime);

        // Rotate the current direction
        Vector2 newDir = Quaternion.Euler(0, 0, rotateStep) * currentDir;

        // Apply new velocity
        rb.linearVelocity = newDir.normalized * spellData.SpellTravelSpeed;
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist <= targetSearchRadious)
            {
                minDist = dist;
                nearest = enemy.transform;
                //Debug.Log($"Found an enemy at {nearest.position}");
            }
        }

        return nearest;
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}