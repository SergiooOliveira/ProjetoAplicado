using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellManagerSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image img_slot;
    [SerializeField] private Transform upgradeSlot;

    private PlayerData playerData;
    private Spell spell;
    private SpellEntry entry;

    private SpellInventoryController spellInventoryController;
    
    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;        
    }

    private void Start()
    {
        spellInventoryController = GetComponentInParent<SpellInventoryController>();
    }

    public void SetSlot(Spell s)
    {
        if (s != null)
        {
            Debug.Log($"Setting slot with spell: {s.RuntimeSpellData.SpellName}");
            spell = s;
            img_slot.enabled = true;
            img_slot.sprite = spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            img_slot.color = spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            Debug.Log("Spell is empty");
            img_slot.enabled = false;
        }
    }

    public void SetSlot(SpellEntry spell)
    {
        if (spell.spell != null)
        {            
            entry = spell;
            img_slot.enabled = true;
            img_slot.sprite = spell.spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            img_slot.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Right clicked on: {(spell != null ? spell.RuntimeSpellData.SpellName : entry.spell.RuntimeSpellData.SpellName)}");

            // Right click
            /*
             * In here be able to Equip/Unequip the player spells
             */
            if (playerData.IsSpellEquipped(spell))
            {
                // Already equipped, so unequip
                playerData.UnequipSpell(playerData.GetSpellSlot(spell));
            }
            else if (spell != null)
            {
                // Try to equip
                //Debug.Log($"Trying to equip: {spell.RuntimeSpellData.SpellName}");
                playerData.EquipSpell(spell);
            }

            //SeeAllSpells();
            spellInventoryController.UpdateUI();
            //spellManager.SetAllSlots();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (upgradeSlot.TryGetComponent<SpellUpgrade>(out SpellUpgrade su))
                su.Initialize(spell);
        }
    }

    private void SeeAllSpells()
    {
        foreach (SpellEntry s in playerData.CharacterEquippedSpells)
        {
            if (s.spell != null)
                Debug.Log($"Equipped: {s.spell.RuntimeSpellData.SpellName}");
            else
                Debug.Log("Equipped: Empty Slot");
        }
    }
}
