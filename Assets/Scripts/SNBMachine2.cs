using UnityEngine;
using UnityEngine.SceneManagement;

public class SNBMachine2 : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Machine clicked!");
        SceneManager.LoadScene("LateSNB");//scene name
    }
}