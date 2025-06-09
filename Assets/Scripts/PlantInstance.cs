using UnityEngine;

public class PlantInstance : MonoBehaviour
{
    public string plantName;
    public Vector3Int tilePosition;
    public ItemData seedData;
    [SerializeField] private GameObject harvestProductPrefab;

    public int CurrentStage { get; private set; } = 0;
    private float growthTimer = 0f;

    private SpriteRenderer spriteRenderer;

    public float GetGrowthTimer() => growthTimer;
    public void RestoreState(int stage, float timer, float timePassed)
    {
        CurrentStage = stage;
        growthTimer = timer + timePassed;
        UpdateVisual();
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 9; // Force render above soil
        }
    }

    private void SpawnHarvestProduct()
    {
        if (seedData.harvestProductPrefab == null)
        {
            Debug.LogError($"No harvest product prefab set for seed: {seedData.itemName}");
            return;
        }

        int yieldCount = Mathf.Max(seedData.harvestYield, 1);
        Debug.Log($"Spawning {yieldCount}x {seedData.harvestProductPrefab.name}");

        for(int i = 0; i < yieldCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            GameObject obj = Instantiate(seedData.harvestProductPrefab, transform.position + randomOffset, Quaternion.identity);

            //  Now this log is inside the loop and sees 'obj'
            Debug.Log($"Spawned object: {obj.name} with position {obj.transform.position}");
        }

        Debug.Log($"[PlantInstance] Spawned {yieldCount}x {seedData.harvestProductPrefab.name} for {plantName}.");
    }


    public void GrowUpdate(float deltaTime)
    {
        if (seedData == null || CurrentStage > 2) return; // > 2 = already harvested or completed

        growthTimer += deltaTime;

        float timePerStage = seedData.harvestTime / 2f;

        if (CurrentStage < 2 && growthTimer >= timePerStage)
        {
            CurrentStage++;
            growthTimer = 0f;
            UpdateVisual();
            return;
        }

        // Only spawn once, when just reaching stage 2 + wait again
        if (CurrentStage == 2 && growthTimer >= timePerStage)
        {
            SpawnHarvestProduct();
            GameManager.instance.tileManager.RemoveGrowthCue(tilePosition);
            Destroy(gameObject, 1f); // destroys plant after 1 second
            growthTimer = 0f;
            CurrentStage++; // mark as done
            Debug.Log($"[GrowUpdate] {plantName} harvested.");
            return;
        }

        Debug.Log($"[GrowUpdate] {plantName} timer: {growthTimer}, stage: {CurrentStage}");
    }


    public void UpdateVisual()
    {
        if (spriteRenderer == null || seedData == null)
        {
            Debug.LogError("[UpdateVisual] Missing SpriteRenderer or seedData!");
            return;
        }

        switch (CurrentStage)
        {
            case 0:
                spriteRenderer.sprite = seedData.seedStageSprite;
                break;
            case 1:
                spriteRenderer.sprite = seedData.sproutStageSprite;
                break;
            case 2:
                spriteRenderer.sprite = seedData.matureStageSprite;
                break;
        }

        GameManager.instance.tileManager.ShowGrowthStage(this);

        Debug.Log($"[PlantInstance] Updated to stage {CurrentStage}");
        Debug.Log($"[UpdateVisual] {plantName} stage: {CurrentStage}, sprite: {spriteRenderer.sprite}");

    }

}
