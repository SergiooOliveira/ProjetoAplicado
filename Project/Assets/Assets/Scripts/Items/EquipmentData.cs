using System.Collections.Generic;
using System.Text;
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
        StringBuilder s = new StringBuilder();

        s.AppendLine("<Color=blue>Leveling UP!!</Color>");

        s.Append($"<Color=purple>Level: </Color><Color=cyan>{CurrentLevel} -> </Color>");
        currentLevel++;
        s.Append($"<Color=lime>{CurrentLevel}\n</Color>");

        s.Append($"<Color=purple>HP Bonus: </Color><Color=cyan>{ItemHpBonus} -> </Color>");
        AddBonusHp(eul.BonusHp);
        s.Append($"<Color=lime>{ItemHpBonus}\n</Color>");

        s.Append($"<Color=purple>Attack Bonus: </Color><Color=cyan>{ItemAttackBonus} -> </Color>");
        AddBonusAttackDamage(eul.BonusAttackDamage);
        s.Append($"<Color=lime>{ItemAttackBonus}\n</Color>");

        s.Append($"<Color=purple>Attack Speed Bonus: </Color><Color=cyan>{ItemAttackSpeedBonus} -> </Color>");
        AddBonusAttackSpeed(eul.BonusAttackSpeed);
        s.Append($"<Color=lime>{ItemAttackSpeedBonus}\n</Color>");

        s.Append($"<Color=purple>Defense Bonus: </Color><Color=cyan>{ItemDefenseBonus} -> </Color>");
        AddBonusDefense(eul.BonusDefense);
        s.Append($"<Color=lime>{ItemDefenseBonus}\n</Color>");

        s.Append($"<Color=purple>Mana Bonus: </Color><Color=cyan>{ItemManaBonus} -> </Color>");
        AddBonusMana(eul.BonusMana);
        s.Append($"<Color=lime>{ItemManaBonus}\n</Color>");

        s.Append($"<Color=purple>Movement Speed Bonus: </Color><Color=cyan>{ItemMovementSpeedBonus} -> </Color>");
        AddBonusMovementSpeed(eul.BonusMovementSpeed);
        s.Append($"<Color=lime>{ItemMovementSpeedBonus}\n</Color>");

        s.Append($"<Color=purple>Bonus Resistances before:\n {EffectsToString(0)}</Color>");
        AddResistanceBonus(eul.BonusResistances);
        s.Append($"<Color=lime>Bonus Resistances After:\n{EffectsToString(1)}\n</Color>");

        s.Append($"<Color=purple>Bonus Damage before:\n {EffectsToString(0)}</Color>");
        AddDamageAffinity(eul.BonusDamageAffinity);
        s.Append($"<Color=lime>Bonus Damage After:\n{EffectsToString(1)}\n</Color>");

        Debug.Log(s);
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
        bool exists = false;
        for (int i = 0; i < damageAffinity.Count; i++)
        {
            for (int j = 0; j < itemDamageAffinity.Count; j++)
            {
                exists = false;
                if (damageAffinity[i].SpellAfinity == itemDamageAffinity[j].SpellAfinity)
                {
                    // This means the resistance already exists
                    itemDamageAffinity[j].AddAmount(damageAffinity[i].Amount);
                    exists = true;
                    break;
                }
            }

            // Resistance does not exist, add new resistance
            if (!exists)
                itemDamageAffinity.Add(damageAffinity[i]);
        }
    }

    private string EffectsToString(int i)
    {
        StringBuilder s = new StringBuilder();

        if (i == 0)
        {
            foreach(Resistance r in ItemResistanceBonus)
            {
                s.AppendLine($"<Color=purple>\t{r.Amount} {r.SpellAfinity}</Color>");
            }

            if (ItemResistanceBonus.Count == 0) s.AppendLine($"<Color=red>\tNo Effects</Color>");

            return s.ToString();
        }
        else
        {
            foreach (Resistance r in ItemResistanceBonus)
            {
                s.AppendLine($"<Color=lime>\t{r.Amount} {r.SpellAfinity}</Color>");
            }

            if (ItemResistanceBonus.Count == 0) s.AppendLine($"<Color=red>\tNo Effects</Color>");

            return s.ToString();
        }
    }
    #endregion
    #endregion
}