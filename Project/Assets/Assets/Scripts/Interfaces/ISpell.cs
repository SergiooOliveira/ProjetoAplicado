using UnityEngine;

public enum SpellTag { Damage, Buff, Debuff }
public enum SpellAffinity { Fire, Wind, Ice, Light, Dark }
public enum SpellCastType { Projectile, Self, Homing, Area, Pierce, Channeled }
public enum SpellImpactType { Damage, Buff, Debuff, Heal, Utility };
public enum SpellManaCostType { Instant, Continuous }

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
    GameObject Cast(Player caster, Vector2 direction);
    #endregion
}