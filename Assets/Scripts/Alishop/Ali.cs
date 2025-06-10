using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MerchantCharacter : MonoBehaviour
{
    private Animator animator;
    public ShopManager shopManager;
    public AudioClip Sound;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // get AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // is no have , add more one
        }
        GameObject Ali = GameObject.Find("Ali");
        if (Ali != null)
            Debug.Log("found ali!");
        else
            Debug.LogWarning("game object ali missing");
        StartCoroutine(DelayedFindShopManager());
    }

    IEnumerator DelayedFindShopManager()
    {
        yield return null; // delay 1 sec to wait others game object to load

        if (shopManager == null)
        {
            // use instance method first
            shopManager = ShopManager.Instance;
        }
    }
    void OnMouseDown()
    {
        if (shopManager != null)
        {
            if (Sound != null)
            {
                audioSource.PlayOneShot(Sound);  // use the sound that inserted
            }
            shopManager.OpenShop();  // open shop
            Debug.Log("Shop Opened"); 
        }
        else
        {
            Debug.LogWarning("ShopManager haven't insterted!");  
        }   
    }
}
