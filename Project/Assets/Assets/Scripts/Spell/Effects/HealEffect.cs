using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Effects/Heal")]
public class HealEffect : SpellEffect
{
    [SerializeField] private int amount;

    public override void Apply(Player caster, Collider2D target)
    {
        if (target == null) return;

        if (target.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.RunTimeData.CharacterHp.IncreaseCurrent(amount);
            return;
        }
        
        if (target.TryGetComponent<Player>(out Player player))
        {
            player.RunTimePlayerData.CharacterHp.IncreaseCurrent(amount);
            return;
        }
    }
}