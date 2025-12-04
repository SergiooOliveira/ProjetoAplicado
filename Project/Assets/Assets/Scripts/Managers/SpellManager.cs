using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private List<Image> slots;

    private PlayerData playerData;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
        SetAllSlots();
    }

    public void SetAllSlots()
    {        
        SetSlot(0);
        SetSlot(1);
        SetSlot(2);        
    }

    private void SetSlot(int index)
    {
        Image img = slots[index];

        if (img == null)
        {
            Debug.LogWarning($"{img} is null on {index}");
            return;
        }        

        SpellEntry entry = playerData.GetSlot(index);

        if (entry.spell != null)
        {
            img.enabled = true;
            img.sprite = entry.spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            img.color = entry.spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().color;

            if (entry.isSelected) img.color = Color.blue;
            else img.color = entry.spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            img.enabled = false;
        }
    }
}
