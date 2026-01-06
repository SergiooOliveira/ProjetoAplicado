using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class TilemapRegionLightSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject freeformLightPrefab;

    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

    void Start()
    {
        GenerateRegionLights();
    }

    void GenerateRegionLights()
    {
        visited.Clear();

        foreach (Vector3Int cell in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(cell) || visited.Contains(cell))
                continue;

            List<Vector3Int> region = FloodFill(cell);
            CreateLightForRegion(region);
        }
    }

    List<Vector3Int> FloodFill(Vector3Int start)
    {
        List<Vector3Int> region = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector3Int[] directions =
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            region.Add(current);

            foreach (var dir in directions)
            {
                Vector3Int next = current + dir;

                if (tilemap.HasTile(next) && !visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
        }

        return region;
    }

    void CreateLightForRegion(List<Vector3Int> region)
    {
        Vector3Int min = region[0];
        Vector3Int max = region[0];

        foreach (var cell in region)
        {
            min = Vector3Int.Min(min, cell);
            max = Vector3Int.Max(max, cell);
        }

        Vector3 minWorld = tilemap.CellToWorld(min);
        Vector3 maxWorld = tilemap.CellToWorld(max + Vector3Int.one);

        Vector3 center = (minWorld + maxWorld) * 0.5f;

        GameObject lightGO = Instantiate(freeformLightPrefab, center, Quaternion.identity, transform);
        Light2D light = lightGO.GetComponent<Light2D>();

        Vector3 size = maxWorld - minWorld;
        Vector3 half = size * 0.5f;

        light.SetShapePath(new Vector3[]
        {
    new Vector3(-half.x, -half.y), // bottom-left
    new Vector3( half.x, -half.y), // bottom-right
    new Vector3( half.x,  half.y), // top-right
    new Vector3(-half.x,  half.y)  // top-left
        });
    }
}
