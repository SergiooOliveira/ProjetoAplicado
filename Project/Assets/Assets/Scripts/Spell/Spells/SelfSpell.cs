using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SelfSpell : MonoBehaviour
{
    private Spell spellData;
    private Player caster;
    
    public void Initialize(Spell spell, Player caster, Transform target)
    {
        this.spellData = spell;
        this.caster = caster;
        
        HealEffect heal = spellData.SpellEffects.OfType<HealEffect>().FirstOrDefault();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Instance.playerTag))
        {
            spellData.OnHit(caster, collision);

            Destroy(gameObject);
        }
    }
}