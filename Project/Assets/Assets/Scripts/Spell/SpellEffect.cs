using UnityEngine;

public abstract class SpellEffect : ScriptableObject
{
    public abstract void Apply(Player caster, Collider2D target);
    public abstract string AddEffectString();
}