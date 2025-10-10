using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    #region Properties

    // *----- Identity -----*    
    string CharacterName { get; }
    int CharacterLevel { get; }
    GameObject CharacterPrefab { get; }

    // *----- Stats -----*
    Stat CharacterHp { get; }
    Stat CharacterXp { get; }
    Stat CharacterMana { get; }

    // *----- Attributes -----*
    float CharacterMovementSpeed { get; }
    float CharacterAttackSpeed { get; }
    int CharacterAttackPower { get; }
    int CharacterDefense { get; }
    List<Resistance> CharacterResistances { get; }

    // *----- Equipables and Inventory -----*
    List<Spell> CharacterEquipedSpells { get; }
    List<Item> CharacterInventory { get; }
    List<Item> CharacterEquipedItems { get; }
    #endregion

    #region Methods
    void AddSpell(Spell spell);
    void RemoveSpell(int slot, Spell spell);
    void SwapSpell(int slot, Spell spellToRemove, Spell spellToAdd);
    #endregion
}
