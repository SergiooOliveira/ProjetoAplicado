using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;

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
        Image img = slots[index].GetComponentInChildren<Image>();
        TMP_Text tb = slots[index].GetComponentInChildren<TMP_Text>();

        if (img == null || tb == null)
        {
            Debug.LogWarning($"{img} is null or {tb} is null on {index}");
            return;
        }        

        SpellEntry entry = playerData.GetSlot(index);

        if (entry.spell != null)
        {            
            //Debug.Log($"{entry.spell.RuntimeSpellData.SpellName} is not null");
         
            img.enabled = true;
            img.sprite = entry.spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            
            if (entry.isSelected) img.color = Color.blue;
            else img.color = Color.white;
            
            tb.text = entry.spell.RuntimeSpellData.SpellName;
        }
        else
        {
            img.enabled = false;
            tb.text = "No spell equipped";
        }
    }
}
