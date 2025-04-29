using UnityEngine.SceneManagement;
using UnityEngine;

public class inventory : MonoBehaviour

{
    public void LoadMainMenu()
    {
        Debug.Log("Go to main page");
        SceneManager.LoadScene("main page"); // Make sure this matches your scene name
    }
}
