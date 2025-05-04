using UnityEngine;

public class CloudController : MonoBehaviour
{
    [System.Serializable]
    public class CloudTile
    {
        public GameObject cloudObject;
        public int unlockLevel;
    }

    public CloudTile[] cloudTiles;

    void Start()
    {
        // Subscribe to level-up event
        LevelSystem.OnLevelUp += OnLevelUp;

        // Also check at start, in case player is already high level
        int currentLevel = FindObjectOfType<LevelSystem>().level;
        CheckCloudUnlock(currentLevel);
    }

    void OnLevelUp(int newLevel)
    {
        CheckCloudUnlock(newLevel);
    }

    void CheckCloudUnlock(int currentLevel)
    {
        foreach (var tile in cloudTiles)
        {
            if (tile.cloudObject != null && currentLevel >= tile.unlockLevel)
            {
                tile.cloudObject.SetActive(false); // Unlock land
            }
        }
    }

    void OnDestroy()
    {
        LevelSystem.OnLevelUp -= OnLevelUp; // Clean up
    }
}
