using UnityEngine;

public class DeveloperTools : MonoBehaviour
{
    [ContextMenu("Reset All Save Data")]
    public void ResetAllSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All save data has been reset. Restart the game to reinitialize everything.");
    }

    [ContextMenu("Reset Only Inventory")]
    public void ResetInventoryInitialization()
    {
        PlayerPrefs.DeleteKey("InventoryInitialized");
        PlayerPrefs.Save();
        Debug.Log("Inventory reset. Restart the game to reinitialize inventory.");
    }
}
