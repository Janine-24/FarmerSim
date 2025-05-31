using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowSNBMachine : MonoBehaviour

{
    public void LoadInventory()
    {
        Debug.Log("Go to inventory");
        SceneManager.LoadScene("InventoryManager"); // Make sure this matches your scene name
    }
}

