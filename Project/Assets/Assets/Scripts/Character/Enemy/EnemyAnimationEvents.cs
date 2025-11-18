using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    #region Fields

    [SerializeField] private Enemy enemy;
    [SerializeField] private List<AttackHitbox> hitboxes = new List<AttackHitbox>();

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

    // Called by animation with a string parameter = hitbox name
    public void EnableHitbox(string hitboxName)
    {
        foreach (var hb in hitboxes)
        {
            if (hb.name == hitboxName)
            {
                hb.EnableHitbox();
                return;
            }
        }
    }

    public void DisableHitbox(string hitboxName)
    {
        foreach (var hb in hitboxes)
        {
            if (hb.name == hitboxName)
            {
                hb.DisableHitbox();
                return;
            }
        }
    }

    #endregion
}