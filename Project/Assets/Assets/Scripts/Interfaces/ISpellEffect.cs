using UnityEngine;

public interface ISpellEffect
{
    void Apply(Player caster, Vector3 castPosition, Vector2 direction);
}