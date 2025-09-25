using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }


}
