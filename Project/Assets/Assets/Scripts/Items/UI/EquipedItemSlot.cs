using UnityEngine;
using UnityEngine.EventSystems;

public class EquipedItemUI : MonoBehaviour, IPointerClickHandler
{
    private PlayerData player;
    private StatManagerUI statManager;

    private void Awake()
    {
        player = GetComponentInParent<Player>().RunTimePlayerData;
        statManager = GetComponentInParent<StatManagerUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            int index = player.CharacterEquipedEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemSlot.ToString() == gameObject.name);

            if (index >= 0)
            {
                player.UnequipEquipment(player.CharacterEquipedEquipment[index]);
                statManager.UpdateUI();
            }
            else
                Debug.LogWarning("Nothing found");
        }
    }
}
