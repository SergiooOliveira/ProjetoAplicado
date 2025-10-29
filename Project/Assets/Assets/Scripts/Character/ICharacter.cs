using JetBrains.Annotations;
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
    List<Equipment> CharacterEquipedItems { get; }
    int CharacterGold { get; }
    #endregion

    #region Spell Methods
    void AddSpell(Spell spell);
    void RemoveSpell(int slot, Spell spell);
    void SwapSpell(int slot, Spell spellToRemove, Spell spellToAdd);
    #endregion

    #region Inventory Methods
    void AddItem(Item item, int amount);
    void RemoveItem(int slot, Item item);
    void SellItem(int slot, Item item);
    void AddGold(int amount);
    #endregion

    #region Equipment Methods
    void AddBonusHp(int amount);
    void AddBonusAttack(int amount);
    void AddBonusAttackSpeed(float amount);
    void AddBonusDefense(int amount);
    void AddBonusMana(int amount);
    void AddBonusMovementSpeed(float amount);
    #endregion
}
