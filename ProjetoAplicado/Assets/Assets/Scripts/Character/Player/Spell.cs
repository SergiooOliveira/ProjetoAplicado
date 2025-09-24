using UnityEngine;

public class Spell : MonoBehaviour
{
    // *----- Identity -----*
    /// <summary>
    /// Name of the Spell
    /// </summary>
    public string SpellName { get; set; }

    /// <summary>
    /// Description of the Spell
    /// </summary>
    public string SpellDescription { get; set; }

    /// <summary>
    /// Tag of the Spell
    /// </summary>
    public string SpellTag { get; set; }

    /// <summary>
    /// Icon of the Spell (Sprite)
    /// </summary>
    public Sprite SpellIcon { get; set; }

    /// <summary>
    /// Afinity of the Spell
    /// </summary>
    public string SpellAfinity { get; set; }

    // *----- Mechanics -----*

    /// <summary>
    /// Damage of the Spell
    /// </summary>
    public int SpellDamage { get; set; }

    /// <summary>
    /// Range that the Spell can trave until it disapears
    /// </summary>
    public int SpellRange { get; set; }

    /// <summary>
    /// Speed that the Spell Travels
    /// </summary>
    public float SpellTravelSpeed { get; set; }

    /// <summary>
    /// Radious of the spell when it collides
    /// </summary>
    public float SpellRadious { get; set; }

    /// <summary>
    /// The time, in seconds, that must pass after casting
    /// before the spell can be used again.
    /// </summary>
    public float SpellCooldown { get; set; }

    /// <summary>
    /// Is the spell instant or does it have a cast time
    /// </summary>
    public float SpellCastSpeed { get; set; }

    /// <summary>
    /// Mana cost for the spell
    /// </summary>
    public int SpellCost { get; set; }

    /// <summary>
    /// Duration the spell in the target (0 if not aplicable)
    /// </summary>
    public float SpellDuration { get; set; }

    // *----- Status Effects -----*
    /// <summary>
    /// Does the spell have AoE
    /// </summary>
    public bool SpellHasAoe { get; set; }

    /// <summary>
    /// Does the spell have CC
    /// </summary>
    public bool SpellHasCC { get; set; }

    /// <summary>
    /// Does the spell have Buff
    /// </summary>
    public bool SpellHasBuff { get; set; }

    /// <summary>
    /// Does the spell have Debuff
    /// </summary>
    public bool SpellHasDebuff { get; set; }
}
