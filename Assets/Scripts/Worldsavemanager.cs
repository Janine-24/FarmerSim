using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BuildingData
{
    public Vector2 localPosition;
    public string buildingType;
    public int level;
}

[System.Serializable]
public class ChunkData
{
    public List<BuildingData> buildings = new();
}

public class WorldSaveManager : MonoBehaviour
{
    public static WorldSaveManager Instance;
    public Dictionary<Vector2Int, ChunkData> savedChunks = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SaveChunk(Vector2Int coord, GameObject chunkObj)
    {
        ChunkData data = new ChunkData();
        foreach (var b in chunkObj.GetComponentsInChildren<Building>())
        {
            data.buildings.Add(new BuildingData
            {
                localPosition = b.transform.localPosition,
                buildingType = b.buildingType,
                level = b.level
            });
        }
        savedChunks[coord] = data;
    }

    public ChunkData LoadChunk(Vector2Int coord)
    {
        return savedChunks.ContainsKey(coord) ? savedChunks[coord] : null;
    }
}
