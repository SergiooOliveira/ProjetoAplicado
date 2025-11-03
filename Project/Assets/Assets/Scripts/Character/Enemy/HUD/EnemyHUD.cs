using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private HealthBarController healthBar;

    private Enemy targetEnemy;

    public void Init(Enemy enemy)
    {
        targetEnemy = enemy;

        // Atualiza os textos uma vez
        nameText.text = enemy.RunTimeData.CharacterName;
        levelText.text = "Lvl " + enemy.RunTimeData.CharacterLevel;
    }

    private void LateUpdate()
    {
        if (targetEnemy == null) return;

        // Faz o HUD seguir o inimigo
        transform.position = targetEnemy.transform.position + offset;

        // Atualiza a barra de vida
        float normalizedHp = (float)targetEnemy.RunTimeData.CharacterHp.Current / targetEnemy.RunTimeData.CharacterHp.Max;
        healthBar.SetHealth(normalizedHp);
    }
}
