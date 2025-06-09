using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPCSkin : MonoBehaviour
{
    private void OnMouseDown()//not ui 
    {
        SceneManager.LoadScene("SkinInterface");
    }
}
