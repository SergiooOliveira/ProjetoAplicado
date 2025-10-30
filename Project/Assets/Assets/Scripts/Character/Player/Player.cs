using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerData runTimePlayerData;

    #region Unity Methods
    public void Awake ()
    {        
        runTimePlayerData = Instantiate(playerData);
        Initialize();

        Debug.Log($"RunTimePlayer: {runTimePlayerData.CharacterName} has the following items");
        foreach (EquipmentEntry equipment in runTimePlayerData.CharacterEquipItems)
        {
            Debug.Log($"{equipment.equipment.RunTimeEquipmentData.ItemName} is {(equipment.isEquipped == true ? "Equipped" : "Not Equipped")}");
        }
    }

    public void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    /// <summary>
    /// Call this method to initialize the player data
    /// </summary>
    public void Initialize()
    {
        Debug.Log("Inside Player Initialize");
        runTimePlayerData.CharacterHp.Initialize();
        runTimePlayerData.CharacterMana.Initialize();
        runTimePlayerData.ClearSpellList();

        foreach (InventoryItem item in runTimePlayerData.CharacterInventory)
        {
            item.item.Initialize();
        }

        foreach (EquipmentEntry equipment in runTimePlayerData.CharacterEquipItems)
        {
            equipment.equipment.Initialize();
        }
    }

    /// <summary>
    /// Public getter for run time player data
    /// </summary>
    public PlayerData RunTimePlayerData => runTimePlayerData;

    public int GetPlayerLevel() => runTimePlayerData.CharacterLevel;
    #endregion
}