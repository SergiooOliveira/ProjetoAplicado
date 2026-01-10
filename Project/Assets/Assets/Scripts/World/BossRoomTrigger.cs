using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{

    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorsCloseSound;
    [SerializeField] private AudioClip doorsOpenSound;

    [SerializeField] private GameObject doorsParent;
    private bool roomLocked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (roomLocked) return;

        if (other.CompareTag("Player"))
        {
            roomLocked = true;

            if (doorsCloseSound != null)
                doorAudioSource.PlayOneShot(doorsCloseSound);

            foreach (var col in doorsParent.GetComponentsInChildren<Collider2D>())
                col.enabled = true;

            foreach (var anim in doorsParent.GetComponentsInChildren<Animator>())
                anim.SetTrigger("Close");
        }
    }

    public void OpenDoors()
    {
        if (doorsOpenSound != null)
            doorAudioSource.PlayOneShot(doorsOpenSound);

        foreach (var anim in doorsParent.GetComponentsInChildren<Animator>())
            anim.SetTrigger("Open");

        foreach (var col in doorsParent.GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}
