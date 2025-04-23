using UnityEngine;

public class PlantingSystem : MonoBehaviour
{
    public Grid grid;                  // Reference to your Grid object
    public Camera mainCamera;         // Camera for mouse click detection
    public SeedItem selectedSeed;     // This is set by the toolbar UI

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedSeed != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);
            Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);

            GameObject obj = Instantiate(selectedSeed.cropPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned: " + obj.name);

        }
    }
}
