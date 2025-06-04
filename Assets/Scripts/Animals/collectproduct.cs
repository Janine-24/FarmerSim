using UnityEngine;

public class Collectproduct : MonoBehaviour
{
    public int rewardAmount = 1; // set the number of product collect like collect 1 bacon insceen, appear 2 bacon in SNB marchine
    public AudioClip collectSound; 

    private void OnMouseDown()
    {
        Collect();
        // sound
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }

    void Collect()
    {
        Debug.Log("Collected");
        Destroy(gameObject, collectSound != null ? collectSound.length : 0f);  // disapear in scene
    }
}
