using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Player Health Bar")]
    [SerializeField] private Image playerHPForeground;

    [Header("Player Mana Bar")]
    [SerializeField] private Image playerManaForeground;

    [Header("Player Xp Bar")]
    [SerializeField] private Image playerXPForeground;

    [SerializeField] private float smoothSpeed = 5f;

    [Header("Textboxes")]
    [SerializeField] private TMP_Text tb_level;
    [SerializeField] private TMP_Text tb_gold;

    private PlayerData playerData;
    private float targetHPFill = 1f;
    private float targetManaFill = 1f;
    private float targetXPFill = 0f;
    
    private void Awake()
    {
        playerData = GetComponentInParent<Player>().RunTimePlayerData;
    }

    public void SetHPBar(float amount)
    {
        targetHPFill = Mathf.Clamp01(amount);
    }

    public void SetManaBar(float amount)
    {
        targetManaFill = Mathf.Clamp01(amount);
        //Debug.Log($"targetManaFill: {targetManaFill}");
    }

    public void SetXPBar(float amount)
    {
        targetXPFill += amount;
        //Debug.Log($"<Color=Lime>targetXPFill: {targetXPFill}</Color>");
    }

    public void ResetXPBar(float amount)
    {
        targetXPFill = amount;
    }

    private void FixedUpdate()
    {
        if (playerHPForeground != null) playerHPForeground.fillAmount = Mathf.Lerp(playerHPForeground.fillAmount, targetHPFill, Time.deltaTime * smoothSpeed);
        if (playerManaForeground != null) playerManaForeground.fillAmount = Mathf.Lerp(playerManaForeground.fillAmount, targetManaFill, Time.deltaTime * smoothSpeed);
        tb_level.text = playerData.CharacterLevel.ToString();
        tb_gold.text = playerData.CharacterGold.ToString();

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
        //Debug.Log("Level Up!");
        targetXPFill -= 1.0f;
        playerXPForeground.fillAmount = 0f;
    }
}