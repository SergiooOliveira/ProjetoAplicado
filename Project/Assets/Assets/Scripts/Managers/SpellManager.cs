using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

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
        
    }
}
