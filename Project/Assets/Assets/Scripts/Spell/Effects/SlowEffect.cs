using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Slow")]
public class SlowEffect : SpellEffect
{
    [Range(0f, 100f)]
    [SerializeField] private float slowPercent;
    [SerializeField] private float duration;

    private bool isEffectApplied = false;

    public override void Apply(Player caster, Collider2D target)
    {
        if (target == null) return;

        if (target.TryGetComponent<Enemy>(out Enemy enemy) && !isEffectApplied)
        {
            isEffectApplied = true;
            enemy.StartCoroutine(ApplySlow(enemy));
        }
        //else
        //{
        //    Debug.Log($"Slow already applied: {isEffectApplied}\n" +
        //        $"Enemy is null: {enemy.RunTimeData.CharacterName}");
        //}
    }

    private IEnumerator ApplySlow(Enemy enemy)
    {
        enemy.RunTimeData.AddBonusMovementSpeed(-slowPercent);
        //Debug.Log($"Enemy movement speed: <Color=lime>{enemy.RunTimeData.CharacterMovementSpeed}</Color>");

        yield return new WaitForSeconds(duration);

        enemy.RunTimeData.AddBonusMovementSpeed(slowPercent);
        //Debug.Log($"Enemy movement speed: <Color=red>{enemy.RunTimeData.CharacterMovementSpeed}</Color>");

        isEffectApplied = false;
    }
}