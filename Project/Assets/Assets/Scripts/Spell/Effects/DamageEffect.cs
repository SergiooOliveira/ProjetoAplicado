using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Damage")]
public class DamageEffect : SpellEffect
{
    [SerializeField] private float baseDamage;

    public override void Apply(Player caster, Collider2D target)
    {
        if (!target.TryGetComponent<Enemy>(out Enemy enemy)) return;

        Spell spell = caster.RunTimePlayerData.GetSlot(caster.RunTimePlayerData.GetActiveSpellIndex()).spell;

        DamageContext context = new DamageContext
        {
            baseDamage = baseDamage,
            caster = caster,
            spell = spell,
            casterAffinityBonuses = caster.GetAffinityBonuses(spell.SpellAfinity),
            targetAffinityResistances = enemy.GetResistance(spell.SpellAfinity)
        };

        enemy.TakeDamage(context);
    }
}