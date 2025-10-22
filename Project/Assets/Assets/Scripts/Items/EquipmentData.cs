using UnityEngine;

public enum EquipmentSlot { Helmet, Chestplate, Leggings, Shoes, Sword, Wand }

[CreateAssetMenu(menuName = "Items/New Equipment")]
public class EquipmentData : ItemData
{
    #region Serialized Fields
    [Header("Bonus Stats")]
    [SerializeField] private float itemHpBonus;                 // Item Hp Bonus
    [SerializeField] private float itemAttackBonus;             // Item Attack Bonus
    [SerializeField] private float itemAttackSpeedBonus;        // Item Attack Speed Bonus in percentage
    [SerializeField] private float itemDefenseBonus;            // Item Defense Bonus
    [SerializeField] private float itemManaBonus;               // Item Mana Bonus
    [SerializeField] private float itemMovementSpeedBonus;      // Item Movement Speed Bonus in percentage
    [SerializeField] private Resistance itemResistanceBonus;    // Item Resistance Bonus in percentage

    [Header("Equiped Flag")]
    [SerializeField] private bool isItemEquiped;
    #endregion

    #region Property implementation
    // *----- Bonus Stats -----*
    public float ItemHpBonus => itemHpBonus;
    public float ItemAttackBonus => itemAttackBonus;
    public float ItemAttackSpeedBonus => itemAttackSpeedBonus;
    public float ItemDefenseBonus => itemDefenseBonus;
    public float ItemManaBonus => itemManaBonus;
    public float ItemMovementSpeedBonus => itemMovementSpeedBonus;
    public Resistance ItemResistanceBonus => itemResistanceBonus;

    // *----- Equiped Flag -----*
    public bool IsItemEquiped => isItemEquiped;
    #endregion
}