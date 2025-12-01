using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private SpellData spellData;
    private SpellData runtimeSpellData;

    public SpellData RuntimeSpellData => runtimeSpellData;

    private List<ScriptableObject> runtimeSpellEffects;
    private float currentMultiplier = 1f;

    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
        this.runtimeSpellEffects = new List<ScriptableObject>();

        foreach(SpellEffect effect in spellData.SpellEffects)
        {
            runtimeSpellEffects.Add(Object.Instantiate(effect));
        }
    }

    public void InitializeRuntimeData()
    {
        if (spellData == null) return;

        runtimeSpellData = Instantiate(spellData);
        List<ScriptableObject> instantiatedEffects = new List<ScriptableObject>();

        foreach (SpellEffect effect in spellData.SpellEffects)
        {
            SpellEffect clone = Instantiate(effect);

            if (clone is DamageEffect dmgEffect)
            {
                dmgEffect.Initialize();
            }

            instantiatedEffects.Add(clone);
        }
    }

    public void SetMultiplier(float multiplier)
    {
        this.currentMultiplier = multiplier;
    }

    public float GetCurrentCost()
    {
        return spellData.SpellCost * currentMultiplier;
    }

    public void OnHit(Player caster, Collider2D target)
    {        
        foreach (SpellEffect effect in runtimeSpellEffects)
        {
            if (effect is DamageEffect damageEffect)
            {
                // TODO: Set damage value in damageEffect
                //damageEffect. = damage;
                damageEffect.Apply(caster, target);
            }
            else
            {
                effect.Apply(caster, target);
            }
        }
    }
}