using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickLSkinC : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SkinInterface";

    private void OnMouseDown()
    {
        SceneManager.LoadScene("SkinInterface");
    }
}
