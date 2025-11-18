using System.Collections;
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
    public void Initialize(DebuffSpell spell, Vector2 direction, Player player)
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
            rb.linearVelocity = direction.normalized * spellData.SpellTravelSpeed;
        }

        StartCoroutine(LifetimeRoutine());
    }

    public IEnumerator LifetimeRoutine()
    {
        Debug.Log("Starting lifetime routine");
        yield return new WaitForSeconds(lifeTimer);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grid")) return;

        // Only affect enemies
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                ApplyDebuff(enemy);
                Destroy(gameObject);
            }
        }
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
            Debug.LogWarning($"{enemy.name} n�o possui StatusEffectController para receber debuffs.");
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