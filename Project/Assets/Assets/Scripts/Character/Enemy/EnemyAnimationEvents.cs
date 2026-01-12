using System.Collections.Generic;
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

    #region Idle
    public void EnterIdleState()
    {
        enemy.PlayIdleSound();
    }

    public void ExitIdleState()
    {
        enemy.StopIdleSound();
    }
    #endregion

    #region Death
    // Death Animation
    public void OnDeathAnimationEnd()
    {
        if (enemy != null)
        {
            enemy.OnDeathAnimationEnd();
        }
    }

    public void PlayDeathSound()
    {
        if (enemy != null)
            enemy.PlayDeathSound();
    }
    #endregion

    #region Attack
    // Called by animation with a string parameter = hitbox name
    public void EnableHitbox(string hitboxName)
    {
        enemy.EnableHitbox(hitboxName);
    }

    public void DisableHitbox(string hitboxName)
    {
        enemy.DisableHitbox(hitboxName);
    }

    public void PlayAttackSound(int index)
    {
        if (enemy == null) return;
        var attack = enemy.CurrentAttack;
        if (attack == null) return;
        if (attack.attackSounds == null || attack.attackSounds.Count == 0) return;

        // Garante que o index não ultrapassa a lista
        index = Mathf.Clamp(index, 0, attack.attackSounds.Count - 1);

        var clip = attack.attackSounds[index];
        if (clip == null) return;

        if (enemy.EffectsSource != null)
            enemy.EffectsSource.PlayOneShot(clip, 1f);
    }
    #endregion

    #endregion
}