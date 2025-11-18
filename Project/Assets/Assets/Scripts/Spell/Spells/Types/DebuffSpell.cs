using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Debuff Spell")]
public class DebuffSpell : Spell
{
    [Header("Debuff Properties")]
    [SerializeField] private DebuffType debuffType;
    [SerializeField] private float debuffValue; // intensidade do efeito (ex: -30% speed)

    public DebuffType DebuffType => debuffType;
    public float DebuffValue => debuffValue;

    public override void Cast(Vector3 position, Vector2 direction, Player player)
    {
        if (SpellPrefab == null)
        {
            Debug.LogWarning($"Spell prefab is null in {SpellName}");
            return;
        }

        //Vector3 spawnPos = position + (Vector3)direction.normalized * 0.5f;
        GameObject instance = Instantiate(SpellPrefab, player.transform.position, Quaternion.identity);

        if (instance.TryGetComponent<SpellDebuff>(out SpellDebuff debuff))
        {
            debuff.Initialize(this, direction, player);
        }
    }
}
