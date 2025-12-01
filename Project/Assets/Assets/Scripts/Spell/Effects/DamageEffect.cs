using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Damage")]
public class DamageEffect : SpellEffect
{
    [SerializeField] private float spellDamage;
    private float currentDamage;
    
    public float SpellDamage => currentDamage;
    
    public void Initialize()
    {
        this.currentDamage = spellDamage;
    }

    public void SetDamageMultiplier(float multiplier)
    {
       this.currentDamage = spellDamage * multiplier;
    }

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