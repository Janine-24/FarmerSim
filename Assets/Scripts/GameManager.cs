using UnityEngine;

// This script handles daily refresh logic for the buying machine
public class GameManager : MonoBehaviour
{
    public BuyingMachineManager buyingMachineManager;  // Reference to the buying machine

    private void Start()
    {
        // Get today's date as a string
        string today = System.DateTime.Now.ToShortDateString();

        // Load the last refresh date from PlayerPrefs
        string lastRefresh = PlayerPrefs.GetString("LastRefreshDate", "");

        // If the last refresh date is not today, refresh the machine
        if (lastRefresh != today)
        {
            buyingMachineManager.RefreshDailyStock();  // Refresh product stocks
            PlayerPrefs.SetString("LastRefreshDate", today);  // Save today's date
        }
    }
}


