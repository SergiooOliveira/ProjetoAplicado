using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellManagerSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image img_slot;
    [SerializeField] private TMP_Text tb_name;

    private PlayerData playerData;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
    }

    public void SetSlot(Spell spell)
    {
        img_slot.sprite = spell.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        tb_name.text = spell.SpellName;
    }

    public void SetSlot(SpellEntry spell)
    {
        if (spell.spell != null)
        {
            img_slot.sprite = spell.spell.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
            tb_name.text = spell.spell.SpellName;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Right click
            /*
             * In here be able to Equip/Unequip the player spells
             */
        }
    }
}
