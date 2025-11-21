using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
    float CharacterAttackPower { get; }
    float CharacterDefense { get; }
    List<Resistance> CharacterResistances { get; }

    // *----- Equipables and Inventory -----*
    List<Spell> CharacterSpells { get; }
    List<ItemEntry> CharacterInventory { get; }
    List<EquipmentEntry> CharacterEquipment { get; }
    int CharacterGold { get; }
    #endregion

    #region Spell Methods
    void AddSpell(Spell spell); // To add a spell to the characterList
    void EquipSpell(Spell spell);
    void UnequipSpell(int slot);
    void SwapSpell(Spell spellToRemove, Spell spellToAdd);
    #endregion

    #region Inventory Methods
    void AddItem(ItemEntry entry, int amount);
    ItemEntry RemoveItem(ItemEntry entry, int amount);    
    void AddEquip(EquipmentEntry equipment);
    EquipmentEntry RemoveEquip(EquipmentEntry equipment);
    void AddGold(int amount);
    #endregion

    #region Equipment Methods
    void EquipEquipment(EquipmentEntry equipment);
    void UnequipEquipment(EquipmentEntry equipment);
    void SwapEquipment(EquipmentEntry equipmentToAdd, EquipmentEntry equipmentToRemove);
    #endregion

    #region Stat Methods
    void EquipmentStats();
    void AddEquipmentStats(EquipmentData equipment);
    void RemoveEquipmentStats(EquipmentData equipment);
    void AddBonusHp(int amount);
    void AddBonusAttack(int amount);
    void AddBonusAttackSpeed(float amount);
    void AddBonusDefense(int amount);
    void AddBonusMana(int amount);
    void AddBonusMovementSpeed(float amount);
    #endregion
}
