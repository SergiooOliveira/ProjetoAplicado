using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

public class TilemapFreeformLightSpawner : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;
    public GameObject freeformLightPrefab;

    [Header("Light Settings")]
    public Vector2 tileSize = Vector2.one;
    public bool generateOnStart = true;

    void Start()
    {
        if (generateOnStart)
            GenerateLights();
    }

    [ContextMenu("Generate Lights")]
    public void GenerateLights()
    {
        if (!tilemap || !freeformLightPrefab)
        {
            Debug.LogError("Tilemap ou prefab da luz não atribuído!");
            return;
        }

        foreach (Vector3Int cellPos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(cellPos))
                continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);

            GameObject lightGO = Instantiate(
                freeformLightPrefab,
                worldPos,
                Quaternion.identity,
                transform
            );

            SetupFreeformLight(lightGO.GetComponent<Light2D>());
        }
    }

    void SetupFreeformLight(Light2D light)
    {
        if (!light || light.lightType != Light2D.LightType.Freeform)
            return;

        Vector2 half = tileSize * 0.5f;

        Vector3[] shape = new Vector3[]
        {
            new Vector3(-half.x, -half.y),
            new Vector3(-half.x,  half.y),
            new Vector3( half.x,  half.y),
            new Vector3( half.x, -half.y)
        };

        light.SetShapePath(shape);
    }
}