using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellUpgradeLevel
{
    [Header("Cost")]
    [SerializeField] private int costGold;
    [SerializeField] private List<ItemCost> itemCost;

    [Header("Upgrades")]
    [SerializeField] private float bonusCooldown;
    [SerializeField] private int bonusCost;
    [SerializeField] private float bonusCastTime;
    [SerializeField] private List<ScriptableObject> bonusEffects;
}