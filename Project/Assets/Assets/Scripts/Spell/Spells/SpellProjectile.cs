using UnityEngine;


public class SpellProjectile : MonoBehaviour
{
    protected ProjectileSpell spellData;
    protected Rigidbody2D rb;

    public virtual void Initialize(ProjectileSpell spell, Vector2 direction)
    {
        spellData = spell;
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = direction.normalized * spellData.SpellTravelSpeed;

        if (spellData.SpellTravelSpeed > 0f && spellData.SpellRange > 0f )
            Destroy(gameObject, spellData.SpellRange / spellData.SpellTravelSpeed);
        else
            Destroy(gameObject, spellData.SpellRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        
        switch (spellData.SpellProjectileType)
        {
            case SpellProjectileType.Normal:
                NormalSpellProjectile(collision);
                break;
            case SpellProjectileType.Pierce:
                break;
            case SpellProjectileType.Explosive:
                break;
            case SpellProjectileType.Chain:
                break;

            default:
                Debug.Log("No Spell projectile type selected");
                break;
        }

        Destroy(gameObject);
    }

    private void NormalSpellProjectile(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            enemy.CalculateDamage(spellData);

    }
}
