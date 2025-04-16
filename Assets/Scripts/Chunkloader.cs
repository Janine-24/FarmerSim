using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChunkLoader : MonoBehaviour
{
    public Transform player;
    public ChunkManager chunkManager;
    public int loadRadius = 1;

    private Vector2Int lastChunk;

    void Update()
    {
        Vector2Int currentChunk = ChunkHelper.GetChunkCoord(player.position);
        if (currentChunk == lastChunk) return;
        lastChunk = currentChunk;

        // Load new chunks
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int coord = currentChunk + new Vector2Int(x, y);
                chunkManager.LoadChunk(coord);
            }
        }

        // Unload far chunks
        foreach (var coord in chunkManager.loadedChunks.Keys.ToList())
        {
            if (Vector2Int.Distance(coord, currentChunk) > loadRadius + 1)
            {
                chunkManager.UnloadChunk(coord);
            }
        }
    }
}
