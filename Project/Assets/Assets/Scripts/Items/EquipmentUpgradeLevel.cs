using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class EquipmentUpgradeLevel
{
    [Header("Cost")]
    [SerializeField] private int costGold;
    [SerializeField] private List<ItemCost> costItem;

    [Header("Upgrades")]
    [SerializeField] private int bonusHp;
    [SerializeField] private float bonusAttackDamage;
    [SerializeField] private float bonusAttackSpeed;
    [SerializeField] private float bonusDefense;
    [SerializeField] private int bonusMana;
    [SerializeField] private float bonusMovementSpeed;
    [SerializeField] private List<Resistance> bonusResistances;
    [SerializeField] private List<Resistance> bonusDamageAffinity;

    // *----- Cost -----*
    public int CostGold => costGold;
    public List<ItemCost> CostItem => costItem;

    // *----- Upgrades -----*
    public int BonusHp => bonusHp;
    public float BonusAttackDamage => bonusAttackDamage;
    public float BonusAttackSpeed => bonusAttackSpeed;
    public float BonusDefense => bonusDefense;
    public int BonusMana => bonusMana;
    public float BonusMovementSpeed => bonusMovementSpeed;
    public List<Resistance> BonusResistances => bonusResistances;
    public List<Resistance> BonusDamageAffinity => bonusDamageAffinity;

    public override string ToString()
    {
        StringBuilder s = new StringBuilder();

        s.AppendLine("Trying to upgrade with the following stats:");
        s.AppendLine($"<Color=cyan>HP: </Color><Color=lime> {BonusHp}</Color>");
        s.AppendLine($"<Color=cyan>Attack Damage: </Color><Color=lime> {BonusAttackDamage}</Color>");
        s.AppendLine($"<Color=cyan>Attack Speed: </Color><Color=lime> {BonusAttackSpeed}</Color>");
        s.AppendLine($"<Color=cyan>Defense: </Color><Color=lime> {BonusDefense}</Color>");
        s.AppendLine($"<Color=cyan>Mana: </Color><Color=lime> {BonusMana}</Color>");
        s.AppendLine($"<Color=cyan>Movement Speed: </Color><Color=lime> {BonusMovementSpeed}</Color>");
        
        foreach (Resistance res in BonusResistances)
        {
            s.AppendLine($"<Color=cyan>Resistance: </Color><Color=lime> {res.Amount} % {res.SpellAfinity}</Color>");
        }

        foreach (Resistance res in BonusDamageAffinity)
        {
            s.AppendLine($"<Color=cyan>Damage Affinity: </Color><Color=lime> {res.Amount} % {res.SpellAfinity}</Color>");
        }

        return s.ToString();
    }
}