using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Info and stats of enemy
    // TODO: Shouldn't be a MonoBehaviour. EnemyManager should be the Monobehaviour
    public EnemyData enemyData;
    
    public void Interact()
    {
        Debug.Log(enemyData.ToString());
    }

    /// <summary>
    /// This method should handle the move factor of the enemy
    /// </summary>
    public void Move()
    {

    }

    /// <summary>
    /// Call this method for Enemy to use the spell
    /// </summary>
    /// <param name="position">Player position</param>
    /// <param name="direction">Player direction</param>
    public void UseSpell(Vector3 position, Vector2 direction)
    {
        enemyData.EnemySpell.Cast(position, direction);
    }

    /// <summary>
    /// Call this method to make the enemy attack
    /// </summary>
    public void Attack()
    {
        switch (enemyData.EnemyType)
        {
            case EnemyType.Ground:
                GroundAttack();
                break;
            case EnemyType.Flying:
                FlyingAttack();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Call this method to handle a ground enemy attack
    /// </summary>
    private void GroundAttack()
    {

    }

    /// <summary>
    /// Call this method to handle a flying enemy attack
    /// </summary>
    private void FlyingAttack()
    {

    }

    /// <summary>
    /// Call this method to give damage to the enemy
    /// </summary>
    /// <param name="damageReceived">Damage Received</param>
    public void CalculateDamage(int damageReceived)
    {
        // Step 1: Percentile defense reduction
        float defenseMultiplier = 100f / (100f + enemyData.EnemyDefense);
        float baseDamage = damageReceived * defenseMultiplier;

        // Step 2: Resistance Multiplier
        float resistanteMultiplier = 1f - (enemyData.GetResistance() / 100f);
        baseDamage *= resistanteMultiplier;

        // Step 3: Level disadvantage penalties
        int levelDifference = enemyData.EnemyLevel - Player.Instance.Level;
        if (levelDifference >= 5) baseDamage *= 0.75f;          // -25%
        else if (levelDifference >= 3) baseDamage *= 0.90f;     // -10%

        // Step 4: Clamp damage and apply
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage));

        enemyData.EnemyHp.TakeDamage(finalDamage);
        if (enemyData.EnemyHp.Current == 0) Die();
    }

    private void Die()
    {
        EnemyManager.Instance.RemoveEnemy(enemyData);

        foreach (Item item in enemyData.EnemyDrops)
        {
            int dropNumber = Random.Range(0, enemyData.EnemyLevel * 2);

            // Add to Player Inventory
        }
    }    
}