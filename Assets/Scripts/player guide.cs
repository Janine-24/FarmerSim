using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuideScene : MonoBehaviour

{
    public void LoadPlayerGuide()
    {
        GameStateManager.Instance.SaveMapState();
        Debug.Log("Go to player guide");
        SceneManager.LoadScene("playerGuideScene"); // Make sure this matches your scene name
    }
}

