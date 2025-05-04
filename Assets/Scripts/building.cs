using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;


public class Building : MonoBehaviour
{
    public string buildingType;
    public int level = 1;
}
public class BuildingPlacer : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    public void PlaceBuilding(string buildingType, Vector3 worldPos)
    {
        var prefab = buildingPrefabs.First(p => p.name == buildingType);
        var building = Instantiate(prefab, worldPos, Quaternion.identity);
        var chunkCoord = Chunk.GetChunkCoord(worldPos);
        var localPos = Chunk.GetLocalPos(worldPos, chunkCoord);

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





