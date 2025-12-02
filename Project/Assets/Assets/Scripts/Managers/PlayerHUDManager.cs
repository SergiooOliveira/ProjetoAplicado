using TMPro;
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
    [SerializeField] private TMP_Text tb_level;
    [SerializeField] private TMP_Text tb_amount;

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
    }

    public void SetManaValues(float amount)
    {
        targetManaFill = Mathf.Clamp01(amount);        
    }

    public void SetXPValues(float amount)
    {
        targetXPFill += amount;
        Debug.Log($"<Color=Lime>targetXPFill: {targetXPFill}</Color>");
    }

    private void FixedUpdate()
    {
        if (playerHPForeground != null) playerHPForeground.fillAmount = Mathf.Lerp(playerHPForeground.fillAmount, targetHPFill, Time.deltaTime * smoothSpeed);
        if (playerManaForeground != null) playerManaForeground.fillAmount = Mathf.Lerp(playerManaForeground.fillAmount, targetManaFill, Time.deltaTime * smoothSpeed);
        tb_level.text = playerData.CharacterLevel.ToString();
        tb_amount.text = $"{playerData.CharacterXp.Current.ToString()} / {playerData.CharacterXp.Max.ToString()}";

        if (playerXPForeground != null)
        {
            if (playerXPForeground.fillAmount >= 0.99f && targetXPFill >= 1.0f)
            {
                HandleLevelUp();
            }
            else
                playerXPForeground.fillAmount = Mathf.Lerp(playerXPForeground.fillAmount, targetXPFill, Time.deltaTime * smoothSpeed);
        }
    }

    private void HandleLevelUp()
    {
        Debug.Log("Level Up!");
        targetXPFill -= 1.0f;
        playerXPForeground.fillAmount = 0f;
    }
}