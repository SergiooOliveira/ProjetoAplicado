using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Armor Break")]
public class ArmorBreakEffect : SpellEffect
{
    [Range(0, 100f)]
    [SerializeField] private int armorReduce;
    [SerializeField] private int duration;

    private bool isEffectApplied = false;

    public override void Apply(Player caster, Collider2D target)
    {
        if (target == null) return;

        if (target.TryGetComponent<Enemy>(out Enemy enemy) && !isEffectApplied)
        {
            isEffectApplied = true;
            enemy.StartCoroutine(ApplyDefenseBreak(enemy));
        }            
    }

    public IEnumerator ApplyDefenseBreak(Enemy enemy)
    {
        enemy.RunTimeData.AddBonusDefense(-armorReduce);
        Debug.Log($"Reducing defense <Color=red>{enemy.RunTimeData.CharacterDefense}</Color>");

        yield return new WaitForSeconds(duration);

        enemy.RunTimeData.AddBonusDefense(armorReduce);
        Debug.Log($"Reducing defense <Color=lime>{enemy.RunTimeData.CharacterDefense}</Color>");
        isEffectApplied = false;
    }
}