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
    public void TakeDamage(int damageReceived)
    {

    }


    private void Die()
    {
        EnemyManager.Instance.RemoveEnemy(enemyData);
    }
}