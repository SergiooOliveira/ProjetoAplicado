using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellUpgrade : MonoBehaviour
{
    [SerializeField] private Image img_slot;
    [SerializeField] private TMP_Text tb_stats;
    [SerializeField] private Button btn_upgrade;

    public void Initialize(Spell spell)
    {
        img_slot.sprite = spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;

        // if player has enough materials to upgrade, show in green, else red and disabled
        // if spell is max level, show in grey and change text to "Max Level"
    }
}
