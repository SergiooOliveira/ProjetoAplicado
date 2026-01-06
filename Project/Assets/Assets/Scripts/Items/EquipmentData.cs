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
    [SerializeField] private float itemAttackBonus;                       // Item Attack Bonus
    [SerializeField] private float itemAttackSpeedBonus;                // Item Attack Speed Bonus in percentage
    [SerializeField] private float itemDefenseBonus;                      // Item Defense Bonus
    [SerializeField] private int itemManaBonus;                         // Item Mana Bonus
    [SerializeField] private float itemMovementSpeedBonus;              // Item Movement Speed Bonus in percentage
    [SerializeField] private List<Resistance> itemResistanceBonus;      // Item Resistance Bonus in percentage
    [SerializeField] private List<Resistance> itemDamageAffinity;       // Item Afinity Attack Bonus

    [Header("Upgrades")]
    [SerializeField] private int currentLevel;                          // Current equipment level
    [SerializeField] private List<EquipmentUpgradeLevel> upgradeLevels; // List of upgrades levels
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public EquipmentSlot ItemSlot => itemSlot;
    public EquipmentSet ItemSet => itemSet;

    // *----- Bonus Stats -----*
    public int ItemHpBonus => itemHpBonus;
    public float ItemAttackBonus => itemAttackBonus;
    public float ItemAttackSpeedBonus => itemAttackSpeedBonus;
    public float ItemDefenseBonus => itemDefenseBonus;
    public int ItemManaBonus => itemManaBonus;
    public float ItemMovementSpeedBonus => itemMovementSpeedBonus;
    public List<Resistance> ItemResistanceBonus => itemResistanceBonus;
    public List<Resistance> ItemDamageAffinity => itemDamageAffinity;

    // *----- Upgrades -----*
    public int CurrentLevel => currentLevel;
    public List<EquipmentUpgradeLevel> UpgradeLevels => upgradeLevels;
    #endregion

    #region Bonus stats
    public void LevelUp(EquipmentUpgradeLevel eul)
    {
        currentLevel++;        

        AddBonusHp(eul.BonusHp);
        AddBonusAttackDamage(eul.BonusAttackDamage);
        AddBonusAttackSpeed(eul.BonusAttackSpeed);
        AddBonusDefense(eul.BonusDefense);
        AddBonusMana(eul.BonusMana);
        AddBonusMovementSpeed(eul.BonusMovementSpeed);
        AddResistanceBonus(eul.BonusResistances);
        AddDamageAffinity(eul.BonusDamageAffinity);
    }

    #region Flat bonus
    public void AddBonusHp(int amount)
    {
        itemHpBonus += amount;
    }

    public void AddBonusAttackDamage(float amount)
    {
        itemAttackBonus += amount;
    }

    public void AddBonusAttackSpeed(float amount)
    {
        itemAttackSpeedBonus += amount;
    }
    
    public void AddBonusDefense(float amount)
    {
        itemDefenseBonus += amount;
    }
    
    public void AddBonusMana(int amount)
    {
        itemManaBonus += amount;
    }
    
    public void AddBonusMovementSpeed(float amount)
    {
        itemMovementSpeedBonus += amount;
    }
    #endregion
    #region List bonus
    /// <summary>
    /// Add resistance bonus
    /// </summary>
    /// <param name="resistance">resistance to add</param>
    public void AddResistanceBonus(List<Resistance> resistances)
    {
        bool exists = false;
        for (int i = 0; i < resistances.Count; i++)
        {
            for (int j = 0; j < itemResistanceBonus.Count; j++)
            {
                exists = false;
                if (resistances[i].SpellAfinity == itemResistanceBonus[j].SpellAfinity)
                {
                    // This means the resistance already exists
                    itemResistanceBonus[j].AddAmount(resistances[i].Amount);
                    exists = true;
                    break;
                }
            }

            if (!exists)            
                itemResistanceBonus.Add(resistances[i]); // Resistance does not exist, add new resistance
        }
    }

    /// <summary>
    /// Add affinity damage bonus
    /// </summary>
    /// <param name="damageAffinity">Affinity damage to add</param>
    public void AddDamageAffinity(List<Resistance> damageAffinity)
    {
        for (int i = 0; i < damageAffinity.Count; i++)
        {
            for (int j = 0; j < itemDamageAffinity.Count; j++)
            {
                if (damageAffinity[i] == itemDamageAffinity[j])
                {
                    // This means the resistance already exists
                    itemDamageAffinity[j].AddAmount(damageAffinity[i].Amount);
                    break;
                }
            }

            // Resistance does not exist, add new resistance
            itemDamageAffinity.Add(damageAffinity[i]);
        }

    }
    #endregion
    #endregion
}