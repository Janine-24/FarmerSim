using UnityEngine;

public class MerchantCharacter : MonoBehaviour
{
    private Animator animator;
    public ShopManager shopManager;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (shopManager != null)
        {
            shopManager.OpenShop();  // open shop
            Debug.Log("Shop Opened"); 
        }
        else
        {
            Debug.LogWarning("ShopManager haven't insterted!");  
        }   
    }
}
