using UnityEngine;

public class MerchantCharacter : MonoBehaviour
{
    private Animator animator;
    public ShopManager shopManager;
    public AudioClip Sound;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (shopManager != null)
        {
            // sound
            if (Sound != null)
                AudioSource.PlayClipAtPoint(Sound, transform.position);
            shopManager.OpenShop();  // open shop
            Debug.Log("Shop Opened"); 
        }
        else
        {
            Debug.LogWarning("ShopManager haven't insterted!");  
        }   
    }
}
