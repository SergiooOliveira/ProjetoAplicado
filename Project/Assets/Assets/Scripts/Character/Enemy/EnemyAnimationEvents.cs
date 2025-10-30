using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{

    [SerializeField] private Enemy enemy;

    public void ApplyAttackDamage()
    {
        if (enemy != null)
        {
            enemy.ApplyAttackDamage();
        }
    }

    public void OnDeathAnimationEnd()
    {
        if (enemy != null)
        {
            enemy.OnDeathAnimationEnd();
        }
    }
}
