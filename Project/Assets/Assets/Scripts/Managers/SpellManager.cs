using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    [SerializeField] GameObject slot3;

    private PlayerData playerData;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
    }

    public void SetAllSlots()
    {
        DisableAllSlots();

        
    }

    private void DisableAllSlots()
    {
        slot1.SetActive(false);
        slot2.SetActive(false);
        slot3.SetActive(false);
    }
}
