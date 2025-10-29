using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static readonly Dictionary<ItemRarity, float> rarityDropRates = new()
    {
        { ItemRarity.Common, 0.99f },
        { ItemRarity.Rare, 0.40f },
        { ItemRarity.Epic, 0.25f },
        { ItemRarity.Legendary, 0.1f }
    };


    [SerializeField] private ItemData itemData;
    private ItemData runTimeItemData;

    [SerializeField] private int itemQuantity;

    public void Initialize()
    {        
        runTimeItemData = ScriptableObject.Instantiate(itemData);
        // Debug.Log($"Initializing {runTimeItemData.ItemName}");
    }

    public ItemData RunTimeItemData => runTimeItemData;
    public int ItemQuantity => itemQuantity;

    #region Methods
    // TODO: It's not on the runTimeSide

    /// <summary>
    /// Call this method to add items
    /// </summary>
    /// <param name="quantity">Quantity to add</param>
    public void AddQuantity(int quantity)
    {
        itemQuantity += quantity;
    }

    /// <summary>
    /// Call this method to remove items
    /// </summary>
    /// <param name="quantity">Quantity to remove</param>
    public void RemoveQuantity(int quantity)
    {
        itemQuantity = Mathf.Max(0, itemQuantity - quantity);
    }

    public void ResetQuantity()
    {
        itemQuantity = 0;
    }
    #endregion
}