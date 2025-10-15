using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject inventoryPrefab;

    private PlayerData runTimePlayerData;
    private InventoryManagerUI inventoryManagerUI;

    #region Unity Methods
    public void Awake ()
    {        
        runTimePlayerData = Instantiate(playerData);
        Initialize();
    }

    public void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    public void Initialize()
    {
        runTimePlayerData.CharacterHp.Initialize();
        runTimePlayerData.CharacterMana.Initialize();
        runTimePlayerData.ClearSpellList();

        GameObject tempInventory = Instantiate(inventoryPrefab, GameObject.Find("UIRoot").transform);
        inventoryManagerUI = tempInventory.GetComponent<InventoryManagerUI>();

        inventoryManagerUI.Bind(runTimePlayerData);
    }

    /// <summary>
    /// Public getter for run time player data
    /// </summary>
    public PlayerData RunTimePlayerData => runTimePlayerData;

    public int GetPlayerLevel() => runTimePlayerData.CharacterLevel;
    #endregion 
}