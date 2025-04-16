using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour

{
    public void LoadMainMenu()
    {
        Debug.Log("Back to main menu clicked");
        SceneManager.LoadScene("main page"); // ← main scene name 
    }
}
