using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private GameObject doorsParent;
    private bool roomLocked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (roomLocked) return;

        if (other.CompareTag("Player"))
        {
            roomLocked = true;

            // ativa os colliders das portas
            foreach (var col in doorsParent.GetComponentsInChildren<Collider2D>())
                col.enabled = true;

            // toca animação "Close" em todas as portas
            foreach (var anim in doorsParent.GetComponentsInChildren<Animator>())
                anim.SetTrigger("Close");
        }
    }

    public void OpenDoors()
    {
        foreach (var anim in doorsParent.GetComponentsInChildren<Animator>())
            anim.SetTrigger("Open");

        foreach (var col in doorsParent.GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}
