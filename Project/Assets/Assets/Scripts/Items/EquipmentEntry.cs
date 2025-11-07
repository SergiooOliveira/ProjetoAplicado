using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct EquipmentEntry
{
    public Equipment equipment;
    [Min(1)] public int quantity;
    public bool isEquipped;

    public bool isEmpty => equipment == null;
    public void Clear() => equipment = null;
}