using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentUpgradeLevel
{
    [Header("Upgrade Cost")]
    [SerializeField] private int costGold;
    [SerializeField] private List<ItemEntry> costItem;

    [Header("Upgraded Stats")]
    [SerializeField] private float bonusHp;
    [SerializeField] private float bonusAttackDamage;
    [SerializeField] private float bonusAttackSpeed;
    [SerializeField] private float bonusDefense;
    [SerializeField] private float bonusMana;
    [SerializeField] private float bonusMovementSpeed;
    [SerializeField] private List<Resistance> bonusResistances;
    [SerializeField] private List<Resistance> bonusDamageAffinity;

    public int CostGold => costGold;
    public List<ItemEntry> CostItem => costItem;
    public float BonusHp => bonusHp;
    public float BonusAttackDamage => bonusAttackDamage;
    public float BonusAttackSpeed => bonusAttackSpeed;
    public float BonusDefense => bonusDefense;
    public float BonusMana => bonusMana;
    public float BonusMovementSpeed => bonusMovementSpeed;
    public List<Resistance> BonusResistances => bonusResistances;
    public List<Resistance> BonusDamageAffinity => bonusDamageAffinity;
}