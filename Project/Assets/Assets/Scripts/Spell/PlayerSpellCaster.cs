using UnityEngine;

public class PlayerSpellCaster : MonoBehaviour
{
    [SerializeField] private BuffSpell buffSpell;
    [SerializeField] private DebuffSpell debuffSpell;

    private float lastBuffCastTime = -Mathf.Infinity;
    private float lastDebuffCastTime = -Mathf.Infinity;

    private void Update()
    {
        // Botão esquerdo = buff
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= lastBuffCastTime + buffSpell.SpellCooldown)
            {
                buffSpell.Cast(transform.position, Vector2.zero, GetComponent<Player>());
                lastBuffCastTime = Time.time;
            }
            else
            {
                Debug.Log($"{buffSpell.SpellName} ainda em cooldown!");
            }
        }

        // Botão direito = debuff
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time >= lastDebuffCastTime + debuffSpell.SpellCooldown)
            {
                debuffSpell.Cast(transform.position, Vector2.zero, GetComponent<Player>());
                lastDebuffCastTime = Time.time;
            }
            else
            {
                Debug.Log($"{debuffSpell.SpellName} ainda em cooldown!");
            }
        }
    }
}
