using UnityEngine;

public class SpellInventoryController : MonoBehaviour
{
    [SerializeField] private Transform spellAvailableList;
    [SerializeField] private Transform spellEquippedList;
    [SerializeField] private GameObject spellSlot;

    [SerializeField] private Transform slot1;
    [SerializeField] private Transform slot2;
    [SerializeField] private Transform slot3;

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
        UpdateUI();
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

            SpellManagerSlot s1 = slot1.GetComponent<SpellManagerSlot>();
            SpellManagerSlot s2 = slot2.GetComponent<SpellManagerSlot>();
            SpellManagerSlot s3 = slot3.GetComponent<SpellManagerSlot>();

            s1.SetSlot(player.CharacterEquippedSpells[0]);
            s2.SetSlot(player.CharacterEquippedSpells[1]);
            s3.SetSlot(player.CharacterEquippedSpells[2]);
        }
    }

    private void DeleteList()
    {
        foreach (Transform i in spellAvailableList)
        {
            Destroy(i.gameObject);
        }

        //foreach (Transform i in spellEquippedList)
        //{
        //    Destroy(i.gameObject);
        //}
    }

    public void UpdateUI()
    {
        DeleteList();
        SetAllSlot();
    }

}
