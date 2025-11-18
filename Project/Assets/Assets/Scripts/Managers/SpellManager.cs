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
        DisableAllSlots();
        
        SetSlot(0);
        SetSlot(1);
        SetSlot(2);        
    }

    private void SetSlot(int index)
    {
        Debug.LogWarning($"Trying to set: {index}");

        Image img = slots[index].GetComponentInChildren<Image>();
        TMP_Text tb = slots[index].GetComponentInChildren<TMP_Text>();

        if (img == null || tb == null)
        {
            Debug.LogWarning($"{img} is null or {tb} is null on {index}");
            return;
        }
        else
        {
            Debug.LogWarning($"Everything ok for {index}");
        }


        SpellEntry entry = playerData.GetSlot(index);

        if (entry.spell != null)
        {            
            Debug.Log($"{entry.spell.SpellName} is not null");
         
            img.enabled = true;
            img.sprite = entry.spell.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            tb.text = entry.spell.SpellName;
        }
        else
        {
            img.enabled = false;
            tb.text = "No spell equipped";
        }
    }

    private void DisableAllSlots()
    {
        
    }
}
