using UnityEngine;

[System.Serializable]
public struct SpellEntry
{
    public Spell spell;
    public int slot;
    public bool isSelected;

    public bool IsEmpty => spell == null;

    public void Select() => isSelected = true;
    public void Deselect() => isSelected = false;
}