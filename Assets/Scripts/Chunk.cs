using UnityEngine;
using System.Linq;

public class Chunk : MonoBehaviour
{
    public Vector2Int chunkCoord;
    public GameObject[] buildingPrefabs;
    public GameObject[] decorations; //trees, rocks, etc.
    public void Generate()
    {
        var data = WorldSaveManager.Instance.LoadChunk(chunkCoord);
        if (data != null)
        {
            // can add buildings
        }

        // randomly place tree, rocks.... by not saved
        for (int i = 0; i < 5; i++)
        {
            Vector2 pos = new Vector2(Random.Range(0, ChunkHelper.chunkSize), Random.Range(0, ChunkHelper.chunkSize));
            Vector3 worldPos = transform.position + (Vector3)pos;

            float checkRadius = 1.5f; // Adjust based on how close is "too close"
            Collider[] nearbyObjects = Physics.OverlapSphere(worldPos, checkRadius);

            bool hasNearbyDecoration = false;
            foreach (var obj in nearbyObjects)
            {
                if (obj.CompareTag("Decoration")) // Make sure your decoration prefabs have this tag
                {
                    hasNearbyDecoration = true;
                    break;
                }
            }

            if (!hasNearbyDecoration)
            {
                var decoPrefab = decorations[Random.Range(0, decorations.Length)];
                Instantiate(decoPrefab, worldPos, Quaternion.identity, transform);
            }

        }

        if (data == null) return;

        foreach (var b in data.buildings)
        {
            GameObject prefab = buildingPrefabs.First(p => p.name == b.buildingType);
            GameObject obj = Instantiate(prefab, transform);
            obj.transform.localPosition = b.localPosition;
            obj.GetComponent<Building>().level = b.level;
        }
    }
}
