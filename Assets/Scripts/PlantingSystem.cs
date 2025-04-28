using UnityEngine;

public class PlantingSystem : MonoBehaviour
{
    public Grid grid;                          // Reference to your Grid object
    public Camera mainCamera;                 // Camera for mouse click detection
    public SeedItem selectedSeed;             // This is set by the toolbar UI
    public GameObject placementPreviewPrefab; // Transparent crop preview

    private GameObject previewInstance;

    void Update()
    {
        if (selectedSeed != null)
        {
            UpdatePlacementPreview();
        }
        else if (previewInstance != null)
        {
            Destroy(previewInstance);
        }

        if (Input.GetMouseButtonDown(0) && selectedSeed != null)
        {
            Vector3Int cellPosition = grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);
            spawnPosition.z = 3f;

            GameObject obj = Instantiate(selectedSeed.cropPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned: " + obj.name);
        }
    }

    void UpdatePlacementPreview()
    {
        Vector3Int cellPosition = grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        Vector3 previewPosition = grid.GetCellCenterWorld(cellPosition);
        previewPosition.z = 3f; // Set Z to 3 for preview too

        if (previewInstance == null)
        {
            previewInstance = Instantiate(placementPreviewPrefab, previewPosition, Quaternion.identity);
        }
        else
        {
            previewInstance.transform.position = previewPosition;
        }
    }
}
