using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Damage")]
public class DamageEffect : SpellEffect
{
    [SerializeField] private float spellDamage;
    private float addedFlatAmount;
    private float damageMultiplier = 1f;
    
    
    //private float currentDamage;
    
    public float SpellDamage => (spellDamage + addedFlatAmount) * damageMultiplier;
    
    public void Initialize()
    {
        addedFlatAmount = 0;
        damageMultiplier = 1f;
    }

    public void SetDamageMultiplier(float multiplier)
    {
       damageMultiplier = multiplier;
    }

    public void AddFlatDamage(float amount)
    {
        addedFlatAmount += amount;
    }

    public override void Apply(Player caster, Collider2D target)
    {
        if (!target.TryGetComponent<Enemy>(out Enemy enemy)) return;

        Spell spell = caster.RunTimePlayerData.GetSlot(caster.RunTimePlayerData.GetActiveSpellIndex()).spell;

        DamageContext context = new DamageContext
        {
            baseDamage = SpellDamage,
            caster = caster,
            spell = spell,
        };

        enemy.TakeDamage(context);
    }

    public override string GetEffectID()
    {
        return "Damage";
    }

    public override string AddEffectString()
    {
        return $"+ {spellDamage} damage";
    }

    public override void Refresh(SpellEffect newSpellEffect)
    {
        StringBuilder s = new StringBuilder();
        if (newSpellEffect is DamageEffect dmg)
        {
            s.AppendLine($"<Color=lime>Damage before: {spellDamage}</Color>");
            s.AppendLine($"<Color=lime>Flat amount before: {addedFlatAmount}</Color>");
            s.AppendLine($"<Color=lime>SpellDamage before: {SpellDamage}</Color>");
            AddFlatDamage(dmg.SpellDamage);
            s.AppendLine($"<Color=red>Damage after: {spellDamage}</Color>");
            s.AppendLine($"<Color=red>Flat amount after: {addedFlatAmount}</Color>");
            s.AppendLine($"<Color=red>SpellDamage after: {SpellDamage}</Color>");
        }

        Debug.Log(s);
    }
}