using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Damage")]
public class DamageEffect : SpellEffect
{
    [SerializeField] private float spellDamage;
    [SerializeField] private float spellRange;
    [SerializeField] private float spellProjectileSpeed;

    public float SpellDamage => spellDamage;
    public float SpellRange => spellRange;
    public float SpellProjectileSpeed => spellProjectileSpeed;

    public override void Apply(Player caster, Collider2D target)
    {
        if (!target.TryGetComponent<Enemy>(out Enemy enemy)) return;

        Spell spell = caster.RunTimePlayerData.GetSlot(caster.RunTimePlayerData.GetActiveSpellIndex()).spell;

        DamageContext context = new DamageContext
        {
            baseDamage = spellDamage,
            caster = caster,
            spell = spell,
        };

        enemy.TakeDamage(context);
    }
}