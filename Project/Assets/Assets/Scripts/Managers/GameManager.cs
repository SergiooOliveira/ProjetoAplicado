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

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);        
    }

    public void Start()
    {
        //player.Initialize("Player", 6f, 0, 0.5f, 0, 0, 1);
    }

    public List<Player> Players => players;

    public void RegisterPlayer(Player player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }
}

/*
 * TODO: Define Items
 * TODO: Continue Interface (Maybe change it for 3 Scriptble Object)
 * TODO: Make an enemy interface (Scriptble Object as well)
*/