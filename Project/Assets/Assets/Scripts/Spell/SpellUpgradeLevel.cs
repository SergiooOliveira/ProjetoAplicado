using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellUpgradeLevel
{
    [Header("Cost")]
    [SerializeField] private int costGold;
    [SerializeField] private List<ItemCost> upgradeItemCost;

    [Header("Upgrades")]
    [SerializeField] private float bonusCooldown;
    [SerializeField] private int bonusManaCost;
    [SerializeField] private float bonusCastTime;
    [SerializeField] private List<ScriptableObject> bonusEffects;

    /* -------- Cost -------- */
    public int CostGold => costGold;
    public List<ItemCost> UpgradeItemCost => upgradeItemCost;

    /* -------- Upgrades -------- */
    public float BonusCooldown => bonusCooldown;
    public int BonusManaCost => bonusManaCost;
    public float BonusCastTime => bonusCastTime;
    public List<ScriptableObject> BonusEffects => bonusEffects;
}