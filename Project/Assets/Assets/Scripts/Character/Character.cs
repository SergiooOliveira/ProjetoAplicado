using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Properties
    private Stat hp { get; set; } = new Stat(100);
    private Stat xp { get; set; } = new Stat(0);
    private Stat mana { get; set; } = new Stat(50);
    
    public string CharacterName { get; private set; }

    public float MovementSpeed { get; private set; }

    public int Resistances { get; set; }

    public float AttackSpeed { get; set; }

    public int Attack { get; private set; }

    public int Defense { get; private set; }

    public int Level { get; private set; }

    public List<Spell> EquipedSpells { get; private set; } = new List<Spell>();
    #endregion

    #region Methods
    public void Initialize(
        string characterName,
        float movementSpeed,
        int resistances,
        float attackSpeed,
        int attack,
        int defense,
        int level)
    {
        this.CharacterName = characterName;
        this.MovementSpeed = movementSpeed;
        this.Resistances = resistances;
        this.AttackSpeed = attackSpeed;
        this.Attack = attack;
        this.Defense = defense;
        this.Level = level;

        this.xp.NewXpMax(this.Level); // Calling this so Xp.Max != 0
    }

    /// <summary>
    /// Call this method to add a Spell to the player spell book
    /// TODO: Change this to add an int with the slot to place said spell
    /// </summary>
    /// <param name="newSpell"></param>
    public void AddSpell (Spell newSpell)
    {
        // Firstly check if Spell is already there
        if (EquipedSpells.Exists(spell => spell.name == newSpell.name)) return;

        // TODO: Check if the limit is already passed, limit it to 3, and keep the order even if it's deleted. Maybe needs to change to array
        
        this.EquipedSpells.Add(newSpell);
        if (EquipedSpells.Count == 1) EquipedSpells[0].isSpellSelected = true;
        Debug.Log("Added: "+ newSpell.name);
    }

    /// <summary>
    /// Call this method to remove a Spell from the player spell book
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spell">Spell to remove</param>
    public void RemoveSpell(int slot, Spell spell)
    {

    }

    /// <summary>
    /// Call this method to swap a spell to another spell
    /// </summary>
    /// <param name="slot">Spell slot in the player spell book</param>
    /// <param name="spellToRemove">Spell to remove</param>
    /// <param name="spellToAdd">Spell to add</param>
    public void SwapSpell (int slot, Spell spellToRemove, Spell spellToAdd)
    {
        RemoveSpell(slot, spellToRemove);
        AddSpell(spellToAdd);
    }
    #endregion
}
