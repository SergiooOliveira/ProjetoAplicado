using UnityEngine;

public abstract class SpellEffect : ScriptableObject
{
    public virtual void Initialize() { }

    public abstract void Apply(Player caster, Collider2D target);
    public abstract string GetEffectID();
    public abstract string AddEffectString();
    public virtual void Refresh(SpellEffect newSpellEffect) { }
}