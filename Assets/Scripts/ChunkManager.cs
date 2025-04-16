using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject[] buildingPrefabs;
    public Dictionary<Vector2Int, GameObject> loadedChunks = new();

    public void LoadChunk(Vector2Int coord)
    {
        if (loadedChunks.ContainsKey(coord)) return;

        GameObject chunkObj = Instantiate(chunkPrefab, ChunkHelper.GetWorldPos(coord), Quaternion.identity);
        Chunk chunk = chunkObj.GetComponent<Chunk>();
        chunk.chunkCoord = coord;
        chunk.buildingPrefabs = buildingPrefabs;
        chunk.Generate();

        loadedChunks[coord] = chunkObj;
    }

    public void UnloadChunk(Vector2Int coord)
    {
        if (!loadedChunks.ContainsKey(coord)) return;

        GameObject chunkObj = loadedChunks[coord];
        WorldSaveManager.Instance.SaveChunk(coord, chunkObj);
        Destroy(chunkObj);
        loadedChunks.Remove(coord);
    }
}
