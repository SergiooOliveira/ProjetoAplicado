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

    public override string GetEffectID()
    {
        return "Slow";
    }

    public override string AddEffectString()
    {
        return $"+ {slowPercent} slow for {duration} seconds";
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

    public override void Refresh(SpellEffect newSpellEffect)
    {
        if (newSpellEffect is SlowEffect slow)
        {
            this.slowPercent += slow.slowPercent;
            this.duration += slow.duration;
        }
    }
}