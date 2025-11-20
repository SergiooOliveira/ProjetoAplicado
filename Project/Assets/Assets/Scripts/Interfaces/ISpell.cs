using UnityEngine;

public enum SpellTag { Damage, Buff, Debuff }
public enum SpellAffinity { Fire, Wind, Ice, Light, Dark }
public enum SpellCastType { Self, Projectile, Area, Targeted }
public enum SpellImpactType { Damage, Buff, Debuff, Heal, Utility };

public interface ISpell
{
    // *----- Identity -----*
    string SpellName { get; }
    string SpellDescription { get; }
    GameObject SpellPrefab { get; }
    SpellTag SpellTag { get; }
    SpellAffinity SpellAfinity { get; }

    // *----- Mechanics -----*
    float SpellCooldown { get; }
    int SpellCost { get; }
    float SpellCastTime { get; }

    SpellCastType SpellCastType { get; }
    SpellImpactType SpellImpactType { get; }
    
    #region Methods
    void Cast(Player caster, Vector2 direction);
    #endregion
}