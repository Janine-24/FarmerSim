using UnityEngine;
using UnityEngine.SceneManagement;

public class SNBMachine : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Machine clicked!");
        SceneManager.LoadScene("SNBMachine2"); 
    }
}
