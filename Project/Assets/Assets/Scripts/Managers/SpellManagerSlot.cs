using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellManagerSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image img_slot;
    [SerializeField] private TMP_Text tb_name;

    private PlayerData playerData;
    private Spell spell;
    private SpellEntry entry;

    private SpellInventoryController spellInventoryController;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
        spellInventoryController = GetComponentInParent<SpellInventoryController>();
    }

    public void SetSlot(Spell s)
    {
        spell = s;
        img_slot.sprite = s.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_name.text = s.SpellName;
    }

    public void SetSlot(SpellEntry spell)
    {
        if (spell.spell != null)
        {            
            entry = spell;
            img_slot.enabled = true;
            img_slot.sprite = spell.spell.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            tb_name.text = spell.spell.SpellName;
        }
        else
        {
            img_slot.enabled = false;
            tb_name.text = "No spell equipped in this slot";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Right clicked on: {(spell != null ? spell.SpellName : entry.spell.SpellName)}");

            // Right click
            /*
             * In here be able to Equip/Unequip the player spells
             */
            if (spell != null)
            {
                // Try to equip
                Debug.Log($"Trying to equip: {spell.SpellName}");
                playerData.EquipSpell(spell);
            }
            else if (entry.spell != null)
            {
                // Try to unequip                
                Debug.Log($"Trying to unequip: {entry.spell.SpellName}");
                playerData.UnequipSpell(entry.slot);
            }

            SeeAllSpells();
            spellInventoryController.UpdateUI();
        }
    }

    private void SeeAllSpells()
    {
        foreach (SpellEntry s in playerData.CharacterEquippedSpells)
        {
            if (s.spell != null)
                Debug.Log($"Equipped: {s.spell.SpellName}");
            else
                Debug.Log("Equipped: Empty Slot");
        }
    }
}
