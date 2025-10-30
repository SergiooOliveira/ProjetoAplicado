using UnityEngine;

[System.Serializable]
public struct EquipmentEntry
{
    public Equipment equipment;
    [Min(1)] public int quantity;
    public bool isEquipped;
}