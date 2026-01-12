using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FluidZone : MonoBehaviour
{
    public float speedMultiplier = 0.5f;
    public float damagePerSecond = 0f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerFluidHandler>(out var player))
        {
            return;
        }

        player.EnterFluid(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerFluidHandler>(out var player))
            return;

        player.ExitFluid(this);
    }
}
