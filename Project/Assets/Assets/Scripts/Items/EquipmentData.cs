using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot { Helmet, Chestplate, Leggings, Shoes, Sword, Wand }

[CreateAssetMenu(menuName = "Items/New Equipment")]
public class EquipmentData : ItemData
{
    #region Serialized Fields
    [Header("Bonus Stats")]
    [SerializeField] private int itemHpBonus;                   // Item Hp Bonus
    [SerializeField] private int itemAttackBonus;               // Item Attack Bonus
    [SerializeField] private float itemAttackSpeedBonus;        // Item Attack Speed Bonus in percentage
    [SerializeField] private int itemDefenseBonus;              // Item Defense Bonus
    [SerializeField] private int itemManaBonus;                 // Item Mana Bonus
    [SerializeField] private float itemMovementSpeedBonus;      // Item Movement Speed Bonus in percentage
    [SerializeField] private List<Resistance> itemResistanceBonus;    // Item Resistance Bonus in percentage

    [Header("Equiped Flag")]
    [SerializeField] private bool isItemEquiped;
    #endregion

    #region Property implementation
    // *----- Bonus Stats -----*
    public int ItemHpBonus => itemHpBonus;
    public int ItemAttackBonus => itemAttackBonus;
    public float ItemAttackSpeedBonus => itemAttackSpeedBonus;
    public int ItemDefenseBonus => itemDefenseBonus;
    public int ItemManaBonus => itemManaBonus;
    public float ItemMovementSpeedBonus => itemMovementSpeedBonus;
    public List<Resistance> ItemResistanceBonus => itemResistanceBonus;

    // *----- Equiped Flag -----*
    public bool IsItemEquiped => isItemEquiped;
    #endregion

    #region Methods
    /// <summary>
    /// Call this method to Unequip an equipment
    /// </summary>
    public void Unequip()
    {
        isItemEquiped = false;
    }
    #endregion
}