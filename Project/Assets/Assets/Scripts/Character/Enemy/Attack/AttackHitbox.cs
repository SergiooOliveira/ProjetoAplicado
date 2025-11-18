using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private bool canDamage = false;
    private PlayerData runTimePlayerData;

    // Chamado pela animação — ativa a hitbox
    public void EnableHitbox()
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


    // Chamado pela animação — desativa a hitbox
    public void DisableHitbox()
    {
        canDamage = false;
    }

    private void ApplyDamage(Collider2D other)
    {
        runTimePlayerData = other.GetComponent<Player>().RunTimePlayerData;

        if (runTimePlayerData == null)
        {
            Debug.Log($"RunTime: {runTimePlayerData.CharacterName}");
            return;
        }

        runTimePlayerData.CharacterHp.TakeDamage(10);

        Debug.Log($"HP: {runTimePlayerData.CharacterHp.Current}");
    }
}