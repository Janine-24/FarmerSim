using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public class Building : MonoBehaviour
{
    public string buildingType;
    public int level = 1;
}

// setting logic for building
public class BuildingPlacer : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    public void PlaceBuilding(string buildingType, Vector3 worldPos)
    {
        var prefab = buildingPrefabs.First(p => p.name == buildingType);
        var building = Instantiate(prefab, worldPos, Quaternion.identity);
        var chunkCoord = ChunkHelper.GetChunkCoord(worldPos);
        var localPos = ChunkHelper.GetLocalPos(worldPos, chunkCoord);

        if (!WorldSaveManager.Instance.savedChunks.ContainsKey(chunkCoord))
        {
            WorldSaveManager.Instance.savedChunks[chunkCoord] = new ChunkData();
        }

        WorldSaveManager.Instance.savedChunks[chunkCoord].buildings.Add(new BuildingData
        {
            localPosition = localPos,
            buildingType = buildingType,
            level = 1
        });
    }
}
