using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InfiniteMapManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject chunkPrefab;
    public GameObject[] buildingPrefabs;

    [Header("Settings")]
    public int chunkSize = 16;
    public int loadRadius = 1;

    private Dictionary<Vector2Int, GameObject> loadedChunks = new();
    private Vector2Int lastPlayerChunk;

    void Start()
    {
        lastPlayerChunk = GetChunkCoord(player.position);
        UpdateChunks();
    }

    void Update()
    {
        Vector2Int current = GetChunkCoord(player.position);
        if (current != lastPlayerChunk)
        {
            lastPlayerChunk = current;
            UpdateChunks();
        }
    }

    //Chunk Logic 

    void UpdateChunks()
    {
        Vector2Int center = GetChunkCoord(player.position);

        // Load nearby chunks
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int coord = center + new Vector2Int(x, y);
                if (!loadedChunks.ContainsKey(coord))
                {
                    LoadChunk(coord);
                }
            }
        }

        // Unload distant chunks
        foreach (var coord in loadedChunks.Keys.ToList())
        {
            if (Vector2Int.Distance(coord, center) > loadRadius + 1)
            {
                UnloadChunk(coord);
            }
        }
    }

    void LoadChunk(Vector2Int coord)
    {
        Vector3 worldPos = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
        GameObject chunk = Instantiate(chunkPrefab, worldPos, Quaternion.identity);
        chunk.name = $"Chunk_{coord.x}_{coord.y}";
        loadedChunks[coord] = chunk;

        // Load saved buildings
        var data = WorldSaveManager.Instance.LoadChunk(coord);
        if (data != null)
        {
            foreach (var b in data.buildings)
            {
                var prefab = buildingPrefabs.First(p => p.name == b.buildingType);
                GameObject obj = Instantiate(prefab, chunk.transform);
                obj.transform.localPosition = b.localPosition;
                obj.GetComponent<Building>().level = b.level;
            }
        }
    }

    void UnloadChunk(Vector2Int coord)
    {
        if (!loadedChunks.TryGetValue(coord, out var chunk)) return;

        WorldSaveManager.Instance.SaveChunk(coord, chunk);
        Destroy(chunk);
        loadedChunks.Remove(coord);
    }

    //  Building Placement 

    public void PlaceBuilding(string type, Vector3 worldPos)
    {
        var chunkCoord = GetChunkCoord(worldPos);
        var localPos = GetLocalPos(worldPos, chunkCoord);

        var prefab = buildingPrefabs.First(p => p.name == type);
        var chunk = loadedChunks[chunkCoord];
        GameObject obj = Instantiate(prefab, chunk.transform);
        obj.transform.localPosition = localPos;

        // Save data
        if (!WorldSaveManager.Instance.savedChunks.ContainsKey(chunkCoord))
        {
            WorldSaveManager.Instance.savedChunks[chunkCoord] = new ChunkData();
        }

        WorldSaveManager.Instance.savedChunks[chunkCoord].buildings.Add(new BuildingData
        {
            buildingType = type,
            level = 1,
            localPosition = localPos
        });
    }

    //  Helpers 

    Vector2Int GetChunkCoord(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / chunkSize),
            Mathf.FloorToInt(worldPos.y / chunkSize)
        );
    }

    Vector2 GetLocalPos(Vector3 worldPos, Vector2Int chunkCoord)
    {
        return new Vector2(
            worldPos.x - chunkCoord.x * chunkSize,
            worldPos.y - chunkCoord.y * chunkSize
        );
    }
}

//  Example Building Script like machine habitat....
public class Building : MonoBehaviour
{
    public string buildingType;
    public int level = 1;
}

//  Save Data Structures avoid missing buildings when loading chunks
[System.Serializable]
public class BuildingData
{
    public string buildingType;
    public int level;
    public Vector2 localPosition;
}

[System.Serializable]
public class ChunkData
{
    public List<BuildingData> buildings = new();
}

//  Virtual Save Manager 
public class WorldSaveManager : MonoBehaviour
{
    public static WorldSaveManager Instance;

    public Dictionary<Vector2Int, ChunkData> savedChunks = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public ChunkData LoadChunk(Vector2Int coord)
    {
        if (savedChunks.TryGetValue(coord, out var data)) return data;
        return null;
    }

    public void SaveChunk(Vector2Int coord, GameObject chunk)
    {
        var data = new ChunkData();
        foreach (Transform child in chunk.transform)
        {
            var building = child.GetComponent<Building>();
            if (building != null)
            {
                data.buildings.Add(new BuildingData
                {
                    buildingType = building.buildingType,
                    level = building.level,s
                    localPosition = child.localPosition
                });
            }
        }
        savedChunks[coord] = data;
    }
}
