using System.Linq;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SpellProjectile : MonoBehaviour
{
    private Spell spellData;
    private Player caster;
    private Rigidbody2D rb;
    private float lifetime;

    public void Initialize(Spell spell, Vector2 dir, Player caster)
    {
        this.spellData = spell;
        this.caster = caster;

        DamageEffect dmg = spell.SpellEffects.OfType<DamageEffect>().FirstOrDefault();

        float speed = dmg?.SpellProjectileSpeed ?? 10f;
        float range = dmg?.SpellRange ?? 10f;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir.normalized * speed;

        if (speed > 0f && range > 0f)
            lifetime = range / speed;
        else if (range > 0f)
            lifetime = range;
        else
            lifetime = 10f;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Instance.playerTag)) return;
        if (collision.CompareTag("Grid")) return;

        spellData.OnHit(caster, collision);
        
        Destroy(gameObject);
    }
}