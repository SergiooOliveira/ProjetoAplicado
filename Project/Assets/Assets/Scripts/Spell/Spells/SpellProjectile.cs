using UnityEngine;


public class SpellProjectile : MonoBehaviour
{
    private ISpell spellData;
    private Vector2 direction;
    private Rigidbody2D rb;

    public void Initialize(ISpell spell, Vector2 castDirection)
    {
        spellData = spell;
        direction = castDirection.normalized;

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * spellData.SpellTravelSpeed;

        Destroy(gameObject, spellData.SpellRange / spellData.SpellTravelSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        Debug.Log($"{spellData.SpellName} hit {collision.name} for {spellData.SpellDamage} damage!");

        switch(spellData.SpellProjectileType)
        {
            case SpellProjectileType.Normal:
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
}
