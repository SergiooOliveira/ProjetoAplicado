using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
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

    public void AddSpell (Spell newSpell)
    {
        // Firstly check if Spell is already there
        if (EquipedSpells.Exists(spell => spell.name == newSpell.name)) return;

        // Check if the limit is already passed
        // Only 3 spells
        // But I want to keep the order

        this.EquipedSpells.Add(newSpell);
        if (EquipedSpells.Count == 1) EquipedSpells[0].isSpellSelected = true;
        Debug.Log("Added: "+ newSpell.name);
    }
}
