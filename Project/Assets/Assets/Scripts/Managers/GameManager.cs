using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Tags
    public readonly string playerTag = "Player";
    public readonly string grimoireTag = "Grimoire";
    public readonly string interactableTag = "Interactable";

    public GameObject player;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void Start()
    {
        player.Initialize("Player", 6f, 0, 0.5f, 0, 0, 1);
    }
}

/*
 * TODO: Define Items
 * TODO: Continue Interface (Maybe change it for 3 Scriptble Object)
 * TODO: Make an enemy interface (Scriptble Object as well)
 */