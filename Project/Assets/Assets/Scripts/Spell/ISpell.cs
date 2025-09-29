using UnityEngine;

public enum SpellTag { Damage, Buff, Debuff }
public enum SpellAfinity { Fire, Wind, Ice, Light, Dark }
public enum SpellProjectileType { Normal, Explosive, Chain, Pierce }

public interface ISpell
{
    // *----- Identity -----*
    string SpellName { get; }
    string SpellDescription { get; }
    SpellTag SpellTag { get; }
    GameObject SpellPrefab { get; }
    SpellAfinity SpellAfinity { get; }

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
    void Cast(Vector3 position, Vector2 direction);
    #endregion
}