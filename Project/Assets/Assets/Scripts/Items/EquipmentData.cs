using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot { Helmet, Chestplate, Leggings, Weapon, Amulet, Ring }
public enum EquipmentSet { Fire, Ice, Wind, Light, Dark }                            

[CreateAssetMenu(menuName = "Items/New Equipment")]
public class EquipmentData : ItemData
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private EquipmentSlot itemSlot;
    [SerializeField] private EquipmentSet itemSet;

    [Header("Bonus Stats")]
    [SerializeField] private int itemHpBonus;                           // Item Hp Bonus
    [SerializeField] private int itemAttackBonus;                       // Item Attack Bonus
    [SerializeField] private float itemAttackSpeedBonus;                // Item Attack Speed Bonus in percentage
    [SerializeField] private int itemDefenseBonus;                      // Item Defense Bonus
    [SerializeField] private int itemManaBonus;                         // Item Mana Bonus
    [SerializeField] private float itemMovementSpeedBonus;              // Item Movement Speed Bonus in percentage
    [SerializeField] private List<Resistance> itemResistanceBonus;      // Item Resistance Bonus in percentage
    [SerializeField] private List<Resistance> itemDamageAffinity;        // Item Afinity Attack Bonus
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public EquipmentSlot ItemSlot => itemSlot;
    public EquipmentSet ItemSet => itemSet;

    // *----- Bonus Stats -----*
    public int ItemHpBonus => itemHpBonus;
    public int ItemAttackBonus => itemAttackBonus;
    public float ItemAttackSpeedBonus => itemAttackSpeedBonus;
    public int ItemDefenseBonus => itemDefenseBonus;
    public int ItemManaBonus => itemManaBonus;
    public float ItemMovementSpeedBonus => itemMovementSpeedBonus;
    public List<Resistance> ItemResistanceBonus => itemResistanceBonus;
    public List<Resistance> ItemDamageAffinity => itemDamageAffinity;
    #endregion
}