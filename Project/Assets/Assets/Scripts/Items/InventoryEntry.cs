using UnityEngine;

[System.Serializable]
public struct InventoryEntry
{
    public Item item;
    [Min(1)]
    public int quantity;
}