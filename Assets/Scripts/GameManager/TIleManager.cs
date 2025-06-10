using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Sprite visualCueForPlant;
    [SerializeField] private Tile interactedTile;
    [SerializeField] private GameObject plantPrefab;

    public List<PlantInstance> plantedCrops = new List<PlantInstance>();
    public HashSet<Vector3Int> hoedTiles = new(); // Track hoed/interacted tiles

    private Dictionary<Vector3Int, GameObject> activeCues = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, GameObject> growthStageCues = new Dictionary<Vector3Int, GameObject>();



    private void Update()
    {
        UpdatePlants();
    }

    private void UpdatePlants()
    {
        foreach (var plant in plantedCrops)
        {
            if (plant == null || plant.seedData == null) continue;

            plant.GrowUpdate(Time.deltaTime);
        }
    }

    public void PlantSeed(Vector3Int position, ItemData seedData)
    {
        if (!NoPlantOnTile(position)) return;

        if (activeCues.ContainsKey(position))
        {
            Destroy(activeCues[position]);
            activeCues.Remove(position);
        }

        Vector3 worldPos = interactableMap.GetCellCenterWorld(position);
        GameObject plantObj = Instantiate(plantPrefab, worldPos, Quaternion.identity);
        PlantInstance instance = plantObj.GetComponent<PlantInstance>();

        if (instance == null)
        {
            Debug.LogError("PlantPrefab is missing the PlantInstance component!");
            Destroy(plantObj);
            return;
        }

        instance.plantName = seedData.itemName;
        instance.tilePosition = position;
        instance.seedData = seedData;

        plantedCrops.Add(instance);
        ShowGrowthStage(instance);
    }


    public void ShowGrowthStage(PlantInstance plant)
    {
        if (plant == null || plant.seedData == null)
        {
            Debug.LogError("Plant or its seedData is null in ShowGrowthStage");
            return;
        }

        Vector3Int tilePos = plant.tilePosition;

        // Destroy old growth cue if it exists
        if (growthStageCues.ContainsKey(tilePos))
        {
            Destroy(growthStageCues[tilePos]);
            growthStageCues.Remove(tilePos);
        }

        //  Create new stage cue GameObject if needed (optional)
        // For example, only do this if you want visible GameObjects on top of the plant
        GameObject cue = new GameObject("GrowthStageCue");
        SpriteRenderer sr = cue.AddComponent<SpriteRenderer>();

        switch (plant.CurrentStage)
        {
            case 0: sr.sprite = plant.seedData.seedStageSprite; break;
            case 1: sr.sprite = plant.seedData.sproutStageSprite; break;
            case 2: sr.sprite = plant.seedData.matureStageSprite; break;
        }

        sr.sortingOrder = 10; // Ensure it's visible
        cue.transform.position = interactableMap.GetCellCenterWorld(tilePos);

        growthStageCues[tilePos] = cue;

        Debug.Log($"[ShowGrowthStage] Spawned cue for {plant.plantName} at stage {plant.CurrentStage}");
    }

    public bool NoPlantOnTile(Vector3Int position)
    {
        return !plantedCrops.Exists(p => p != null && p.tilePosition == position);
    }


    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return "";
    }

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

  

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null)
        {
            if (tile.name == "Interactable")
            {
                return true;
            }
        }
        return false;
    }

    public void ReverseInteracted(Vector3Int position)
    {
        // If there's a cue on this tile, do nothing
        if (activeCues.ContainsKey(position))
        {
            Debug.Log("Cannot reverse tile: visual cue is active.");
            return;
        }

        interactableMap.SetTile(position, hiddenInteractableTile);
        hoedTiles.Remove(position);
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);

        hoedTiles.Add(position);
    }

    public bool IsPlantable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null)
        {
            if (tile.name == "soil")
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveGrowthCue(Vector3Int tilePos)
    {
        if (growthStageCues.ContainsKey(tilePos))
        {
            Destroy(growthStageCues[tilePos]);
            growthStageCues.Remove(tilePos);
            Debug.Log($"[TileManager] Removed growth stage cue at {tilePos}");
        }
    }

    public void RestorePlantedSeed(Vector3Int position, ItemData seedData, int stage, float timer, float timePassed, int waterCount)
    {
        Vector3 worldPos = interactableMap.GetCellCenterWorld(position);
        GameObject plantObj = Instantiate(plantPrefab, worldPos, Quaternion.identity);
        PlantInstance instance = plantObj.GetComponent<PlantInstance>();
        instance.plantName = seedData.itemName;
        instance.tilePosition = position;
        instance.seedData = seedData;
        instance.RestoreState(stage, timer, timePassed, waterCount); // ✅ Fix is here
        plantedCrops.Add(instance);
        ShowGrowthStage(instance);
    }

    public bool ShowPlantingCue(Vector3Int tilePos)
    {
        if (activeCues.ContainsKey(tilePos))
            return false;

        Vector3 worldPos = interactableMap.GetCellCenterWorld(tilePos);

        GameObject cue = new GameObject("PlantingCue");
        SpriteRenderer sr = cue.AddComponent<SpriteRenderer>();
        sr.sprite = visualCueForPlant;
        sr.sortingOrder = 9; // Make sure it appears above tiles

        cue.transform.position = worldPos;

        activeCues[tilePos] = cue;

        return true;
    }
}