using UnityEngine;

[System.Serializable]
public struct ItemEntry
{
    public Item item;
    [Min(1)] public int quantity;
    public bool isGuarantee;

    public bool isEmpty => item == null;
    public void Clear() => item = null;

    public void AddQuantity(int amount) => quantity += amount;
    public void RemoveQuantity(int amount) => quantity -= amount;
}