using UnityEngine;
using UnityEngine.SceneManagement;

public class playerguide ：: MonoBehaviour

{
    public void LoadPlayerGuide()
    {
        SceneManager.LoadScene("playerGuideScence"); // Make sure this matches your scene name
    }
}

