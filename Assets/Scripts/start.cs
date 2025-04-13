using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleScene : MonoBehaviour //base script that let code run in the scene.
{
    public void StartGame() //it will run when you call it (like from a button click or a key press).
    {
        SceneManager.LoadScene("SampleScene"); //this loads the scene called SampleScene. Make sure this matches your scene name.

    void Upadate() //runs every single frame while the game is playing. It’s perfect for checking for key presses, movement
    {
        if (Input.GetKeyDown(KeyCode.Return)) //This checks if the player has pressed the Enter key (Return is Enter). GetKeyDown = only triggers once when the key is first pressed
        {
            Debug.Log("Starting game..."); 
            StartGame(); //game starts when the player presses Enter.
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //This checks if the player has pressed the Escape key
        {
            Debug.Log("Exiting game..."); 
            Application.Quit(); //this quits the application 
        }
    }
    void OpenPlayerGuideScene()
    {
    SceneManager.LoadScene("playerGuideScene"); // to guide scene
    }
    }
}