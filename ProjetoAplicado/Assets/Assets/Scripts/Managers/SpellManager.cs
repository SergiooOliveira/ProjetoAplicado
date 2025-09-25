using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    private Dictionary<string, Spell> spells = new();

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        // TODO: Turn into scriptable object
        spells["Fireball"] = new Spell
        {
            SpellName = "Fireball",
            SpellDescription = "Fireball Description",
            SpellTag = "Damage",
            SpellIcon = LoadSprite("Sprites/Fireball"),
            SpellAfinity = "Fire",
            SpellDamage = 10,
            SpellRange = 10,
            SpellTravelSpeed = 0.5f,
            SpellRadious = 0.2f,
            SpellCooldown = 1f,
            SpellCastSpeed = 0f,
            SpellCost = 10,
            SpellDuration = 0f,
            SpellHasAoe = false,
            SpellHasCC = false,
            SpellHasBuff = false,
            SpellHasDebuff = false
        };
    }

    /// <summary>
    /// Call this method to get the spell
    /// </summary>
    /// <param name="name">Name of the spell</param>
    /// <returns>Returns spell as an object of type Spell</returns>
    public Spell GetSpell(string name) => spells[name];

    private Sprite LoadSprite(string path)
    {
        Sprite loadedSprite = Resources.Load<Sprite>(path);

        //Debug.LogWarning($"Path: {path}");
        if (loadedSprite == null) Debug.LogError($"Error loading {path}");

        return loadedSprite;
    }
}
