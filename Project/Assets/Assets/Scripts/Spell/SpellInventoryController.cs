using UnityEngine;

public class SpellInventoryController : MonoBehaviour
{
    [SerializeField] private Transform spellAvailableList;
    [SerializeField] private Transform spellEquippedList;
    [SerializeField] private GameObject spellSlot;

    private PlayerData player;

    private void Awake()
    {
        player = GetComponentInParent<Player>().RunTimePlayerData;
    }

    private void OnEnable()
    {
        /*
         * Delete list
         * Show playerData available spells and the equipped ones
         */
        DeleteList();
        SetAllSlot();
    }

    private void SetAllSlot()
    {
        if (player != null)
        {
            foreach (Spell spell in player.CharacterSpells)
            {
                GameObject newSlot = Instantiate(spellSlot, spellAvailableList);
                SpellManagerSlot sms = newSlot.GetComponent<SpellManagerSlot>();
                sms.SetSlot(spell);
            }

            for (int i = 0; i < player.CharacterEquippedSpells.Count; i++)
            {
                GameObject newSlot = Instantiate(spellSlot, spellEquippedList);
                SpellManagerSlot sms = newSlot.GetComponent<SpellManagerSlot>();
                sms.SetSlot(player.CharacterEquippedSpells[i]);
            }
        }
    }

    private void DeleteList()
    {
        foreach (Transform i in spellAvailableList)
        {
            Destroy(i.gameObject);
        }

        foreach (Transform i in spellEquippedList)
        {
            Destroy(i.gameObject);
        }
    }


}
