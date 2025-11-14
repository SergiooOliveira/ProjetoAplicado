using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SpellDebuff : MonoBehaviour
{
    private DebuffSpell spellData;
    private Player playerData;

    private CircleCollider2D areaCollider;
    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.right; // default para evitar zero
    private float lifeTimer;

    // Called by the DebuffSpell when instantiated
    public void Initialize(DebuffSpell spell, Player player)
    {
        spellData = spell;
        playerData = player;

        areaCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Collider should be trigger so we can detect hits
        areaCollider.isTrigger = true;

        // Set collider radius from SpellRange (hit radius/area around projectile)
        areaCollider.radius = spell.SpellRange;

        // Life time of the projectile / AoE object
        lifeTimer = spell.SpellDuration;

        // Rigidbody setup: zero gravity, no rotation
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        // Timer for auto-destroy if duration set
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        // Move the projectile by setting velocity (use SpellTravelSpeed)
        if (rb != null && spellData != null)
        {
            rb.velocity = moveDirection.normalized * spellData.SpellTravelSpeed;
        }
    }

    /// <summary>
    /// Call this right after Initialize to set movement direction.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.0001f)
            moveDirection = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only affect enemies
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            ApplyDebuff(enemy);
            Destroy(gameObject);
        }

        // Optionally, ignore Player, Grid, etc.
        if (collision.CompareTag("Player") || collision.CompareTag("Grid")) return;
    }

    private void ApplyDebuff(Enemy enemy)
    {
        if (spellData == null) return;

        if (enemy.TryGetComponent<StatusEffectController>(out StatusEffectController controller))
        {
            controller.ApplyDebuff(spellData.DebuffType, spellData.DebuffValue, spellData.SpellDuration);
        }
        else
        {
            Debug.LogWarning($"{enemy.name} não possui StatusEffectController para receber debuffs.");
        }
    }

    // --- GIZMOS ---
    private void OnDrawGizmos()
    {
        if (areaCollider == null)
            areaCollider = GetComponent<CircleCollider2D>();

        if (areaCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, areaCollider.radius);

            Color fill = Gizmos.color;
            fill.a = 0.15f;
            Gizmos.color = fill;
            Gizmos.DrawSphere(transform.position, areaCollider.radius);
        }
    }
}