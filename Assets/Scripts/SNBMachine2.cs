using UnityEngine;
using UnityEngine.SceneManagement;

public class SNBMachine2 : MonoBehaviour
{
    void OnMouseDown()
    {
        GameStateManager.Instance.SaveMapState();
        Debug.Log("Machine clicked!");
        SceneManager.LoadScene("LateSNB");//scene name
    }
}