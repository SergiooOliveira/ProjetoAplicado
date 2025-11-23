using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Slow")]
public class SlowEffect : SpellEffect
{
    [Range(0f, 100f)]
    [SerializeField] private float slowPercent;
    [SerializeField] private float duration;


    public override void Apply(Player caster, Collider2D target)
    {
        if (target == null) return;

        if (target.TryGetComponent<Enemy>(out Enemy enemy)) return;
        if (enemy.appliedEffects.Contains(this)) return;

        enemy.appliedEffects.Add(this);
        enemy.StartCoroutine(ApplySlow(enemy));
    }

    private IEnumerator ApplySlow(Enemy enemy)
    {
        enemy.RunTimeData.AddBonusMovementSpeed(-slowPercent);
        Debug.Log($"Enemy movement speed: <Color=lime>{enemy.RunTimeData.CharacterMovementSpeed}</Color>");

        yield return new WaitForSeconds(duration);

        enemy.RunTimeData.AddBonusMovementSpeed(slowPercent);
        Debug.Log($"Enemy movement speed: <Color=red>{enemy.RunTimeData.CharacterMovementSpeed}</Color>");

        enemy.appliedEffects.Remove(this);
    }
}