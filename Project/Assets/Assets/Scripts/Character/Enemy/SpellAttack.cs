using UnityEngine;

public class SpellAttack : MonoBehaviour
{
    #region Fields

    [HideInInspector] public Enemy enemy;
    private bool active = false;

    private Collider2D col;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    #endregion

    #region Animation Events

    // This method will be called by the animation event
    public void EnableHitbox()
    {
        active = true;

        Collider2D[] results = new Collider2D[5];
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        int count = col.Overlap(filter, results);

        for (int i = 0; i < count; i++)
        {
            if (results[i].CompareTag("Player"))
            {
                enemy.ApplyDamage(results[i]);
            }
        }
    }

    public void DisableHitbox()
    {
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;

        if (collision.CompareTag("Player"))
        {
            enemy.ApplyDamage(collision);
        }
    }

    public void AttackEnd()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        if (active)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        if (col != null)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }

    #endregion
}