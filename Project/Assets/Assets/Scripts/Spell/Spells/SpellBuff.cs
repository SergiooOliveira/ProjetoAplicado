using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SpellBuff : MonoBehaviour
{
    private BuffSpell spellData;
    private Player caster;

    private CircleCollider2D areaCollider;
    private float timer;

    private float lastBuffCastTime = -Mathf.Infinity;

    public void Initialize(BuffSpell spell, Player player)
    {
        spellData = spell;
        caster = player;

        areaCollider = GetComponent<CircleCollider2D>();
        areaCollider.isTrigger = true;

        // Define o raio da área com base no spell range
        areaCollider.radius = spell.SpellRange;

        // Define tempo de vida
        timer = spell.SpellDuration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var target = collision.GetComponent<StatusEffectController>();
            if (target != null)
                target.ApplyBuff(spellData.BuffType, spellData.BuffValue, spellData.SpellDuration);
        }
    }

    // --- VISUALIZAÇÃO COM GIZMOS ---
    private void OnDrawGizmos()
    {
        if (areaCollider == null)
            areaCollider = GetComponent<CircleCollider2D>();

        // Define cor do gizmo conforme o tipo do buff (opcional)
        Gizmos.color = GetBuffColor();

        // Desenha o círculo do raio da área
        Gizmos.DrawWireSphere(transform.position, areaCollider.radius);

        // Também pode desenhar um leve preenchimento transparente (Editor apenas)
        Color fill = Gizmos.color;
        fill.a = 0.15f;
        Gizmos.color = fill;
        Gizmos.DrawSphere(transform.position, areaCollider.radius);
    }

    private Color GetBuffColor()
    {
        if (spellData == null)
            return Color.cyan;

        switch (spellData.BuffType)
        {
            case BuffType.ArmorBreaker: return Color.green;
            default: return Color.cyan;
        }
    }
}
