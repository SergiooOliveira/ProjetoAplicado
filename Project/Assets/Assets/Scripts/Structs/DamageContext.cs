using System.Collections.Generic;
using UnityEngine;

public struct DamageContext
{
    public float baseDamage;
    public Spell spell;
    public Player caster;

    public float casterAffinityBonuses;
    public float targetAffinityResistances;
    public List<float> temporaryModifiers;
}