using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [HideInInspector] public Enemy owner;
    private bool active = false;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;

        if (owner == null)
        {
            owner = GetComponentInParent<Enemy>();
        }
    }


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
                owner.ApplyDamage(results[i]);
            }
        }
    }

    public void DisableHitbox()
    {
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;

        if (other.CompareTag("Player"))
        {
            owner.ApplyDamage(other);
        }
    }
}