using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    private Spell spellData;
    private Player caster;
    private Rigidbody2D rb;

    public void Initialize(Spell spell, Vector2 dir, Player caster)
    {
        this.spellData = spell;
        this.caster = caster;

        rb.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * spell.SpellProjectileSpeed;

        Destroy(gameObject, spell.SpellRange / spell.SpellProjectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Instance.playerTag)) return;

        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.CalculateDamage(caster, spellData);
        }

        Destroy(gameObject);
    }
}