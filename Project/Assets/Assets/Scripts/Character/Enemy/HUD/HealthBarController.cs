using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image foregroundImage; // Verde
    [SerializeField] private Image backgroundImage; // Vermelho (opcional, s� est�tica)

    private float targetFill = 1f;
    [SerializeField] private float smoothSpeed = 5f;

    /// <summary>
    /// Atualiza a barra de vida, valor entre 0 e 1
    /// </summary>
    public void SetHealth(float normalizedHealth)
    {
        targetFill = Mathf.Clamp01(normalizedHealth);
    }

    private void Update()
    {
        if (foregroundImage != null)
        {
            // Suaviza a transi��o da barra
            foregroundImage.fillAmount = Mathf.Lerp(foregroundImage.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }
    }
}
