using UnityEngine;

[CreateAssetMenu(fileName = "new Spell", menuName = "Spells")]
public class Spell : ScriptableObject
{
    // TODO: Turn into an interface

    public enum SpellTag { Damage, Buff, Debuff }
    public enum SpellAfinity { Fire, Wind, Ice, Light, Dark }

    // *----- Identity -----*
    public string spellName;
    public string spellDescription;
    public SpellTag spellTag;
    public Sprite spellIcon;
    public SpellAfinity spellAfinity;

    // *----- Mechanics -----*

    public int SpellDamage;
    public int SpellRange;
    public float SpellTravelSpeed;
    public float SpellRadious;
    public float SpellCooldown;
    public float SpellCastSpeed;
    public int SpellCost;
    public float SpellDuration;

    // *----- Status Effects -----*
    //public bool SpellHasAoe; 
    public bool SpellHasCC;
    public bool SpellHasBuff;
    public bool SpellHasDebuff;
}
