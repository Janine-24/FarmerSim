using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteTileGenerator : MonoBehaviour
{
    [Header("Tilemap and Tiles")]
    public Tilemap tilemap;
    public Tile[] groundTiles; // Assign multiple tiles in inspector

    [Header("Player")]
    public Transform player;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int viewDistanceInChunks = 2;
    public float noiseScale = 0.1f;

    private Vector2Int lastPlayerChunk;
    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateChunksAroundPlayer();
    }

    void Update()
    {
        Vector2Int currentChunk = GetPlayerChunk();

        if (currentChunk != lastPlayerChunk)
        {
            GenerateChunksAroundPlayer();
            lastPlayerChunk = currentChunk;
        }
    }

    Vector2Int GetPlayerChunk()
    {
        return new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );
    }

    void GenerateChunksAroundPlayer()
    {
        Vector2Int centerChunk = GetPlayerChunk();

        for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++)
        {
            for (int y = -viewDistanceInChunks; y <= viewDistanceInChunks; y++)
            {
                Vector2Int chunkPos = new Vector2Int(centerChunk.x + x, centerChunk.y + y);
                if (!generatedChunks.Contains(chunkPos))
                {
                    GenerateChunk(chunkPos);
                    generatedChunks.Add(chunkPos);
                }
            }
        }
    }

    void GenerateChunk(Vector2Int chunkCoord)
    {
        int startX = chunkCoord.x * chunkSize;
        int startY = chunkCoord.y * chunkSize;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float worldX = startX + x;
                float worldY = startY + y;

                float noiseValue = Mathf.PerlinNoise(worldX * noiseScale, worldY * noiseScale);
                Tile tileToUse = GetTileFromNoise(noiseValue);

                Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(worldX), Mathf.FloorToInt(worldY), 0);
                tilemap.SetTile(tilePos, tileToUse);
            }
        }
    }

    Tile GetTileFromNoise(float noise)
    {
        if (groundTiles == null || groundTiles.Length == 0)
            return null;

        int index = Mathf.FloorToInt(noise * groundTiles.Length);
        index = Mathf.Clamp(index, 0, groundTiles.Length - 1);
        return groundTiles[index];
    }
}
