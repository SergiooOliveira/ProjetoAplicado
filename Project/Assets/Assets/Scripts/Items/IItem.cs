using UnityEngine;

public enum ItemType { Ingredient, Trash, KeyItem }

public interface IItem
{
    #region Properties
    [Header("Identifier")]
    string ItemName { get; }
    string ItemDescription { get; }
    GameObject ItemPrefab { get; }
    ItemType ItemType { get; }

    [Header("Value")]
    bool IsItemSellable { get; }
    int ItemSellValue { get; }    
    #endregion
}