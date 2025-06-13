using UnityEngine;
using UnityEngine.SceneManagement;

public class LateSNB : MonoBehaviour
{
    // use Buying n Selling Machine Manager 
    public BuyingMachineManager buyingMachineManager;
    public SellingMachineManager sellingMachineManager;

    public void Loadmainpage()
    {
        Debug.Log("Go to main page");
        SceneManager.LoadScene("main page");
    }

    public void OnCloseButtonClicked()
    {
        // save buy data
        if (buyingMachineManager != null)
        {
            buyingMachineManager.SaveInventory();
        }
        else
        {
            Debug.LogError("BuyingMachineManager not been use！");
        }

        // save selling data
        if (sellingMachineManager != null)
        {
            sellingMachineManager.SaveInventory();
        }
        else
        {
            Debug.LogError("SellingMachineManager not been use！");
        }

        // change to main page
        SceneManager.LoadScene("main page");
    }
}