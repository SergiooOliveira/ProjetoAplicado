using UnityEngine;

public class SpellAttack : MonoBehaviour
{
    #region Fields

    public int damage = 20;
    private bool canDamage = false;

    #endregion

    #region Animation Events

    // This method will be called by the animation event
    public void EnableDamage()
    {
        canDamage = true;

        // Checks if the player is already inside the collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            // Create a filter that considers all colliders
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter(); // Or filter just "Player" if you wan

            Collider2D[] results = new Collider2D[10];
            int count = col.Overlap(filter, results); // Here we use the new Overlap

            for (int i = 0; i < count; i++)
            {
                if (results[i].CompareTag("Player"))
                {
                    // Player is already in
                    ApplyDamage(results[i]);
                }
            }
        }
    }

    private void ApplyDamage(Collider2D other)
    {
        // TODO: Apply Damage Palyer
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    public void AttackEnd()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        if (canDamage)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }

    #endregion
}