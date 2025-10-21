using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class Item : ScriptableObject, IItem
{
    #region Serialized Fields
    [Header("Identity")]
    [SerializeField] private string itemName;           // Item Name
    [SerializeField] private string itemDescription;    // Item Description
    [SerializeField] private GameObject itemPrefeb;     // Item Prefab
    [SerializeField] private ItemType itemType;         // Item Type
    [SerializeField] private ItemRarity itemRarity;     // Item Rarity
    [SerializeField] private int itemQuantity;          // Item Quantity

    [Header("Value")]
    [SerializeField] private bool isItemSellable;       // Is item Sellable
    [SerializeField] private int itemSellValue;         // Item Sell Value
    #endregion

    #region Property implementation
    // *----- Identity -----*
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public GameObject ItemPrefab => itemPrefeb;
    public ItemType ItemType => itemType;
    public ItemRarity ItemRarity => itemRarity;
    public int ItemQuantity => itemQuantity;

    // *----- Value -----*
    public bool IsItemSellable => isItemSellable;
    public int ItemSellValue => itemSellValue;
    #endregion

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
    #endregion
}