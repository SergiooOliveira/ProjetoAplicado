using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    #region Fields

    [SerializeField] private Enemy enemy;

    #endregion

    #region Animation Event

    // Apply Attack / Effect Damage
    public void ApplyAttackEffect()
    {
        if (enemy != null)
        {
            enemy.ApplyAttackEffect();
        }
    }

    // Death Animation
    public void OnDeathAnimationEnd()
    {
        if (enemy != null)
        {
            enemy.OnDeathAnimationEnd();
        }
    }

    #endregion
}