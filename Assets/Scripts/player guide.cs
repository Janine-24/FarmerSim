using UnityEngine;
using UnityEngine.SceneManagement;

public class playerGuideScene : MonoBehaviour

{
    public void LoadPlayerGuide()
    {
        Debug.Log("Go to player guide");
        SceneManager.LoadScene("playerGuideScene"); // Make sure this matches your scene name
    }
}

