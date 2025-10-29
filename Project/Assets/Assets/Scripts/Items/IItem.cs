using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Ingredient, Trash, KeyItem }
public enum ItemRarity { Common, Rare, Epic, Legendary }

public interface IItem
{
    #region Properties
    [Header("Identifier")]
    string ItemName { get; }
    string ItemDescription { get; }
    GameObject ItemPrefab { get; }
    ItemRarity ItemRarity { get; }

    [Header("Value")]
    bool IsItemSellable { get; }
    int ItemSellValue { get; }
    #endregion
}