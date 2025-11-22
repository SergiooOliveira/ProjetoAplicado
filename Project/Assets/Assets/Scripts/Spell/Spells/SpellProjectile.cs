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

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir.normalized * spell.SpellProjectileSpeed;

        if (spell.SpellProjectileSpeed > 0f && spell.SpellRange > 0f)
            lifetime = spell.SpellRange / spell.SpellProjectileSpeed;
        else if (spell.SpellRange > 0f)
            lifetime = spell.SpellRange;
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