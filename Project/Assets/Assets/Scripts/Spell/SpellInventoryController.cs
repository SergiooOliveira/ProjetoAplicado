using UnityEngine;

public class SpellInventoryController : MonoBehaviour
{
    [SerializeField] private Transform[] slots;

    [SerializeField] private SpellManager spellManager;

    private PlayerData player;

    private void Awake()
    {
        player = GetComponentInParent<Player>().RunTimePlayerData;
    }

    private void OnEnable()
    {
        UpdateUI();
        GameManager.Instance.TogglePause(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.TogglePause(false);
    }

    private void SetAllSlot()
    {
        if (player != null)
        {
            // Percorrer a list de transforms e caso haja um spell preencher o slot
            for (int i = 0; i < slots.Length; i++)
            {
                SpellManagerSlot sms = slots[i].GetComponent<SpellManagerSlot>();

                if (i < player.CharacterSpells.Count)
                {
                    // Spell exist
                    //Debug.Log($"Setting spell slot {i} with spell: {player.CharacterSpells[i].RuntimeSpellData.SpellName}");
                    sms.SetSlot(player.CharacterSpells[i]);
                }
                else
                {
                    // Spell does not exist
                    sms.SetSlot(null);
                }
            }
        }
    }

    public void UpdateUI()
    {
        SetAllSlot();
        spellManager.SetAllSlots();
    }
}
