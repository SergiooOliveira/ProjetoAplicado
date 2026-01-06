using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Tags
    public readonly string playerTag = "Player";
    public readonly string grimoireTag = "Grimoire";
    public readonly string interactableTag = "Interactable";
    public readonly string gridTag = "Grid";
    public readonly LayerMask bossLayer;
    public readonly LayerMask groundLayer;

    [SerializeField] private List<Player> players = new();

    public bool isUiOpen {  get; private set; }

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {

    }

    public List<Player> Players => players;

    public void RegisterPlayer(Player player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void TogglePause(bool isOpen)
    {
        isUiOpen = isOpen;

        if (isOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}