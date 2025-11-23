using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Armor Break")]
public class ArmorBreakEffect : SpellEffect
{
    [Range(0, 100f)]
    [SerializeField] private int armorReduce;
    [SerializeField] private int duration;

    public override void Apply(Player caster, Collider2D target)
    {
        if (target == null) return;

        if (!target.TryGetComponent<Enemy>(out Enemy enemy)) return;
        if (enemy.appliedEffects.Contains(this))
        {
            Debug.Log("Defense break already applied");
            return;
        }

        Debug.Log($"Applying defense break to {enemy.RunTimeData.CharacterName}");
        enemy.appliedEffects.Add(this);
        enemy.StartCoroutine(ApplyDefenseBreak(enemy));        
    }

    public IEnumerator ApplyDefenseBreak(Enemy enemy)
    {
        enemy.RunTimeData.AddBonusDefense(-armorReduce);
        Debug.Log($"Reducing defense <Color=red>{enemy.RunTimeData.CharacterDefense}</Color>");

        yield return new WaitForSeconds(duration);

        enemy.RunTimeData.AddBonusDefense(armorReduce);
        Debug.Log($"Reducing defense <Color=lime>{enemy.RunTimeData.CharacterDefense}</Color>");

        enemy.appliedEffects.Remove(this);
    }
}