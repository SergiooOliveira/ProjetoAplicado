using UnityEngine;

[System.Serializable]
public struct InventoryItem
{
    public Item item;
    [Min(1)] public int quantity;
    public bool isGuarantee;
}