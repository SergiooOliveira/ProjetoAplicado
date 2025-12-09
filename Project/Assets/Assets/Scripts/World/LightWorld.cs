using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightWorld : MonoBehaviour
{
    [Header("Lighting")]
    public Tilemap terrainTilemap;
    public Texture2D worldTilesMap;
    public Material lightShader;
    public float lightThreshold;
    public float lightRadius = 7f;
    List<Vector2Int> unLitBlocks = new List<Vector2Int>();

    public int worldSize = 200;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldTilesMap = new Texture2D(worldSize, worldSize);
        worldTilesMap.filterMode = FilterMode.Point;
        lightShader.SetTexture("_ShadowTex", worldTilesMap);

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                worldTilesMap.SetPixel(x, y, Color.white);
            }
        }
        worldTilesMap.Apply();

        GenerateTerrain();

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (worldTilesMap.GetPixel(x, y) == Color.white)
                    LightBlock(x, y, 1f, 0);
            }
        }
        worldTilesMap.Apply();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateTerrain()
    {
        InitializeLightMap();
        worldTilesMap.Apply();
    }

    void InitializeLightMap()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (TileExistsAt(x, y)) // função que verifica se tem tile
                    worldTilesMap.SetPixel(x, y, Color.black); // escuro = bloqueia luz
                else
                    worldTilesMap.SetPixel(x, y, Color.white); // claro = espaço vazio
            }
        }
        worldTilesMap.Apply();
    }

    bool TileExistsAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);
        TileBase tile = terrainTilemap.GetTile(pos);
        return tile != null;
    }

    void ApplyVerticalDarkness()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                float t = (float)y / worldSize; // 0 no fundo, 1 no topo
                Color current = worldTilesMap.GetPixel(x, y);
                worldTilesMap.SetPixel(x, y, current * t);
            }
        }
        worldTilesMap.Apply();
    }

    public void CheckTile(int x, int y)
    {
        RemoveLightSource(x, y);
    }

    public void RemoveTile(int x, int y)
    {
        worldTilesMap.SetPixel(x, y, Color.white);
        LightBlock(x, y, 1f, 0);

        worldTilesMap.Apply();
    }

    void LightBlock(int x, int y, float intensity, int iteration)
    {
        if (iteration < lightRadius)
        {
            worldTilesMap.SetPixel(x, y, Color.white * intensity);

            for (int nx = x - 1; nx < x + 2; nx++)
            {
                for (int ny = y - 1; ny < y + 2; ny++)
                {
                    if (nx != x || ny != y)
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(nx, ny));
                        float targetIntensity = Mathf.Pow(0.7f, dist) * intensity;
                        if (worldTilesMap.GetPixel(nx, ny) != null)
                        {
                            if (worldTilesMap.GetPixel(nx, ny).r < targetIntensity)
                            {
                                LightBlock(nx, ny, targetIntensity, iteration + 1);
                            }
                        }
                    }
                }
            }

            worldTilesMap.Apply();
        }
    }

    void RemoveLightSource(int x, int y)
    {
        unLitBlocks.Clear();
        UnLightBlock(x, y, x, y);

        List<Vector2Int> toRelight = new List<Vector2Int>();
        foreach (Vector2Int block in unLitBlocks)
        {
            for (int nx = block.x - 1; nx < block.x + 2; nx++)
            {
                for (int ny = block.y - 1; ny < block.y + 2; ny++)
                {
                    if (worldTilesMap.GetPixel(nx, ny) != null)
                    {
                        if (worldTilesMap.GetPixel(nx, ny).r > worldTilesMap.GetPixel(block.x, block.y).r)
                        {
                            if (!toRelight.Contains(new Vector2Int(nx, ny)))
                                toRelight.Add(new Vector2Int(nx, ny));
                        }
                    }
                }
            }
        }

        foreach (Vector2Int source in toRelight)
        {
            LightBlock(source.x, source.y, worldTilesMap.GetPixel(source.x, source.y).r, 0);
        }

        worldTilesMap.Apply();
    }

    void UnLightBlock(int x, int y, int ix, int iy)
    {
        if (Mathf.Abs(x - ix) >= lightRadius || Mathf.Abs(y - iy) >= lightRadius || unLitBlocks.Contains(new Vector2Int(x, y)))
            return;

        for (int nx = -1; nx < x + 2; nx++)
        {
            for (int ny = -1; ny < y + 2; ny++)
            {
                if (nx != x || ny != y)
                {
                    if (worldTilesMap.GetPixel(nx, ny) != null)
                    {
                        if (worldTilesMap.GetPixel(nx, ny).r < worldTilesMap.GetPixel(x, y).r)
                        {
                            UnLightBlock(nx, ny, ix, iy);
                        }
                    }
                }
            }
        }

        worldTilesMap.SetPixel(x, y, Color.black);
        unLitBlocks.Add(new Vector2Int(x, y));
    }
}
