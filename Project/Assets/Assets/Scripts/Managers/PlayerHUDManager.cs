using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Player Health Bar")]
    [SerializeField] private Image playerHPBackground;
    [SerializeField] private Image playerHPForeground;

    [Header("Player Mana Bar")]
    [SerializeField] private Image playerManaBackground;
    [SerializeField] private Image playerManaForeground;

    [Header("Player Xp Bar")]
    [SerializeField] private Image playerXPBackground;
    [SerializeField] private Image playerXPForeground;

    private PlayerData playerData;
    private float targetHPFill = 1f;
    private float targetManaFill = 1f;
    private float targetXPFill = 0f;
    [SerializeField] private float smoothSpeed = 5f;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
    }

    public void SetHPValues(float amount)
    {
        targetHPFill = Mathf.Clamp01(amount);
        if (playerHPForeground != null) playerHPForeground.fillAmount = Mathf.Lerp(playerHPForeground.fillAmount, targetHPFill, Time.deltaTime * smoothSpeed);
    }

    public void SetManaValues(float amount)
    {
        targetManaFill = Mathf.Clamp01(amount);
        if (playerManaForeground != null) playerManaForeground.fillAmount = Mathf.Lerp(playerManaForeground.fillAmount, targetManaFill, Time.deltaTime * smoothSpeed);
    }

    public void SetXPValues(float amount)
    {
        Debug.Log($"Received amount: {amount}");
        targetXPFill = Mathf.Clamp01(amount);
        Debug.Log($"Setting XP Fill to {targetXPFill}");
        if (playerXPForeground != null) playerXPForeground.fillAmount = Mathf.Lerp(playerXPForeground.fillAmount, targetXPFill, Time.deltaTime * smoothSpeed);
    }
}