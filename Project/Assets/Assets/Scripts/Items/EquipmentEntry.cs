using UnityEngine;

[System.Serializable]
public struct EquipmentEntry
{
    public Equipment equipment;
    [Min(1)] public int quantity;
    public bool isEquipped;

    public void Equip() =>   isEquipped = true;
    public void Unequip() => isEquipped = false;
}