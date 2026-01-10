using FishNet.Object;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FluidsAmbientSounds : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public float fadeSpeed = 4f;

    [Header("Distance Settings")]
    public float hearStartDistance = 3f; // distância onde o som começa a ouvir
    public float maxDistance = 6f;       // distância máxima onde o som é 0

    [Header("Tilemap Reference")]
    public Tilemap lavaTilemap;

    [Header("Volume Curve (opcional)")]
    public AnimationCurve volumeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Transform localPlayer;
    private float targetVolume;

    void Awake()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play(); // sempre tocando
    }

    void FixedUpdate()
    {
        // encontra o player local
        if (!localPlayer)
        {
            foreach (var netObj in FindObjectsByType<NetworkObject>(FindObjectsSortMode.None))
            {
                if (netObj.IsOwner)
                {
                    localPlayer = netObj.transform;
                    break;
                }
            }
        }

        if (!localPlayer || !lavaTilemap)
            return;

        float closestDistance = float.MaxValue;

        // percorre todos os tiles de lava
        foreach (Vector3Int cellPos in lavaTilemap.cellBounds.allPositionsWithin)
        {
            if (!lavaTilemap.HasTile(cellPos))
                continue;

            Vector3 worldPos = lavaTilemap.GetCellCenterWorld(cellPos);
            float distance = Vector2.Distance(localPlayer.position, worldPos);

            if (distance < closestDistance)
                closestDistance = distance;
        }

        // calcula targetVolume de forma contínua
        if (closestDistance <= maxDistance)
        {
            float t;

            if (closestDistance <= hearStartDistance)
            {
                t = 1f; // máximo volume
            }
            else
            {
                // normaliza entre hearStartDistance e maxDistance
                t = 1f - ((closestDistance - hearStartDistance) / (maxDistance - hearStartDistance));
                t = Mathf.Clamp01(t);

                // aplica curva se quiser
                t = volumeCurve.Evaluate(t);
            }

            targetVolume = t;
        }
        else
        {
            targetVolume = 0f;
        }

        // fade suave
        audioSource.volume = Mathf.Lerp(
            audioSource.volume,
            targetVolume,
            Time.deltaTime * fadeSpeed
        );
    }
}

