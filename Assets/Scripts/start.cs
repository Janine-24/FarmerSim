using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleScene : MonoBehaviour //base script that lets code run in the scene.
{
    public void StartGame() //it will run when you call it (like from a button click or a key press).
    {
        SceneManager.LoadScene("main page"); //this loads the scene called SampleScene. Make sure this matches your scene name.
    }

    void Update() //runs every single frame while the game is playing. It’s perfect for checking for key presses, movement
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Starting game...");
            StartGame();
            SceneManager.LoadScene("main page");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Debug.Log("Exiting game...");
            Application.Quit(); 
        }
    }
}
