using UnityEngine;

public class Collectproduct : MonoBehaviour
{
    public int rewardAmount = 1; // set the number of bacon if collect like collect 1 bacon insceen, appear 2 bacon in SNB marchine

    private void OnMouseDown()
    {
        Collect();
    }

    void Collect()
    {
        Debug.Log("Collected");
        Destroy(gameObject); // disapear in scene
    }
}
