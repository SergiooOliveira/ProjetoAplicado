using UnityEngine;
using System.Collections.Generic;

public enum Foot
{
    Left,
    Right
}

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource;
    public List<FootstepSet> footstepSets = new List<FootstepSet>();

    private string groundTag;
    private FootstepSet currentSet;

    private bool isInFluid;

    // Detecta chão normal (não trigger)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isInFluid) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                UpdateGround(collision.collider.tag);
                return;
            }
        }
    }

    // Detecta triggers (como água)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInFluid = true;
            UpdateGround("Water");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInFluid = false;
            groundTag = null;
            currentSet = null;
        }
    }

    // Atualiza o terreno atual
    private void UpdateGround(string tag)
    {
        if (groundTag == tag) return;

        groundTag = tag;
        currentSet = footstepSets.Find(s => s.groundTag == groundTag);
    }

    // Chamadas dos Animation Events
    public void StepLeft() => PlayStep(Foot.Left);
    public void StepRight() => PlayStep(Foot.Right);

    private void PlayStep(Foot foot)
    {
        if (currentSet == null) return;

        AudioClip clip = foot switch
        {
            Foot.Left => currentSet.leftFoot != null ? currentSet.leftFoot : currentSet.rightFoot,
            Foot.Right => currentSet.rightFoot != null ? currentSet.rightFoot : currentSet.leftFoot,
            _ => null
        };

        if (clip == null) return;

        //audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}
