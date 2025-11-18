using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Buff Spell")]
public class BuffSpell : Spell
{
    [Header("Buff Properties")]
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue;

    public BuffType BuffType => buffType;
    public float BuffValue => buffValue;

    public override void Cast(Vector3 position, Vector2 direction, Player player)
    {
        if (SpellPrefab == null)
        {
            Debug.LogWarning($"Spell prefab is null in {SpellName}");
            return;
        }

        // Instancia o prefab do campo de buff na posição do player
        GameObject instance = Instantiate(SpellPrefab, position, Quaternion.identity);

        if (instance.TryGetComponent<SpellBuff>(out SpellBuff buff))
            buff.Initialize(this, player);
        else
            Debug.LogWarning($"{SpellName}: prefab {SpellPrefab.name} missing SpellBuff component");
    }
}