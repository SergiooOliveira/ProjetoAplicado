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
    private float targetXPFill = 1f;
    [SerializeField] private float smoothSpeed = 5f;

    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
        UpdateHUD();
    }

    public void SetHPValues(float amount)
    {
        targetHPFill = Mathf.Clamp01(amount);
        UpdateHUD();
    }

    public void SetManaValues(float amount)
    {
        targetManaFill = Mathf.Clamp01(amount);
        UpdateHUD();
    }

    public void SetXPValues(float amount)
    {
        targetXPFill = Mathf.Clamp01(amount);
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (playerHPForeground != null) playerHPForeground.fillAmount = Mathf.Lerp(playerHPForeground.fillAmount, targetHPFill, Time.deltaTime * smoothSpeed);
        if (playerManaForeground != null) playerManaForeground.fillAmount = Mathf.Lerp(playerManaForeground.fillAmount, targetManaFill, Time.deltaTime * smoothSpeed);
        if (playerXPForeground != null) playerXPForeground.fillAmount = Mathf.Lerp(playerXPForeground.fillAmount, targetXPFill, Time.deltaTime * smoothSpeed);        
    }
}