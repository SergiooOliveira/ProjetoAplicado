using System.Collections.Generic;
using UnityEngine;

public struct DamageContext
{
    public float baseDamage;
    public Spell spell;
    public Player caster;

    public List<float> casterAffinityBonuses;
    public List<float> targetAffinityResistances;
    public List<float> temporaryModifiers;
}