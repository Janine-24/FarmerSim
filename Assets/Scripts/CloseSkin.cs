using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseSkin : MonoBehaviour
{
    public void Loadmainpage()
    {
        Debug.Log("Go to main page");
        SceneManager.LoadScene("main page"); //scene name
    }
}
