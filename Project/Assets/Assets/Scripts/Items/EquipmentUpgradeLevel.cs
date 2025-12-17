using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentUpgradeLevel
{
    [Header("Cost")]
    [SerializeField] private int costGold;
    [SerializeField] private List<ItemEntry> costItem;

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
    public List<ItemEntry> CostItem => costItem;

    // *----- Upgrades -----*
    public int BonusHp => bonusHp;
    public float BonusAttackDamage => bonusAttackDamage;
    public float BonusAttackSpeed => bonusAttackSpeed;
    public float BonusDefense => bonusDefense;
    public int BonusMana => bonusMana;
    public float BonusMovementSpeed => bonusMovementSpeed;
    public List<Resistance> BonusResistances => bonusResistances;
    public List<Resistance> BonusDamageAffinity => bonusDamageAffinity;
}