using UnityEngine;

public enum SpellTag { Damage, Buff, Debuff }
public enum SpellAffinity { Fire, Wind, Ice, Light, Dark }
public enum SpellProjectileType { Normal, Explosive, Chain, Pierce }
public enum DebuffType { Slow }
public enum BuffType { ArmorBreaker }

public interface ISpell
{
    // *----- Identity -----*
    string SpellName { get; }
    string SpellDescription { get; }
    SpellTag SpellTag { get; }
    GameObject SpellPrefab { get; }
    SpellAffinity SpellAfinity { get; }
    SpellProjectileType SpellProjectileType { get; }

    // *----- Mechanics -----*
    int SpellDamage { get; }
    int SpellRange { get; }
    float SpellTravelSpeed { get; }
    float SpellCooldown { get; }
    float SpellCastSpeed { get; }
    int SpellCost { get; }
    float SpellDuration { get; }

    // *----- Conditions -----*
    bool IsSpellUnlocked { get; }
    bool IsSpellEquiped { get; }
    bool IsSpellSelected { get; }

    // *----- Status Effects -----*
    bool SpellHasCC { get; }
    bool SpellHasBuff { get; }
    bool SpellHasDebuff { get; }

    #region Methods
    void Cast(Vector3 position, Vector2 direction, Player player);
    #endregion
}