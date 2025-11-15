using UnityEngine;

[System.Serializable]
public class EnemyAttack
{
    [Tooltip("Trigger name in Animator")]
    public string triggerName;

    [Tooltip("Damage this attack does")]
    public int damage;

    [Tooltip("Relative weight for chance of happening")]
    public float weight = 1f;

    [Tooltip("Attack Type")]
    public AttackType attackType;
}