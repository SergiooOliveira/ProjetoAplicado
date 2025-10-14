using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class Item : ScriptableObject, IItem
{
    [Header("Identity")]
    [SerializeField] private string itemName;           // Item Name
    [SerializeField] private string itemDescription;    // Item Description
    [SerializeField] private GameObject itemPrefeb;     // Item Prefab
    [SerializeField] private ItemType itemType;         // Item Type
    [SerializeField] private ItemRarity itemRarity;     // Item Rarity

    [Header("Value")]
    [SerializeField] private bool isItemSellable;       // Is item Sellable
    [SerializeField] private int itemSellValue;         // Item Sell Value

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public GameObject ItemPrefab => itemPrefeb;
    public ItemType ItemType => itemType;
    public ItemRarity ItemRarity => itemRarity;
    public bool IsItemSellable => isItemSellable;
    public int ItemSellValue => itemSellValue;
}