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
    public int spellDamage;
    public int spellRange;
    public float spellTravelSpeed;
    public float spellRadious;
    public float spellCooldown;
    public float spellCastSpeed;
    public int spellCost;
    public float spellDuration;
    public bool isSpellUnlocked;
    public bool isSpellEquiped;
    public bool isSpellSelected;

    // *----- Status Effects -----*
    //public bool SpellHasAoe; 
    public bool spellHasCC;
    public bool spellHasBuff;
    public bool spellHasDebuff;
}
