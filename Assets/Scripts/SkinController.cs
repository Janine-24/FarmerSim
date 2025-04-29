using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinController : MonoBehaviour

{
    public void OpenSkinInterface()
    {
        Debug.Log("Go to skin interface");
        SceneManager.LoadScene("SkinInterface"); // Make sure this matches your scene name
    }
}