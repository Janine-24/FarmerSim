using UnityEngine;

public class Collectproduct : MonoBehaviour
{
    public int rewardAmount = 1; // set the number of product collect like collect 1 bacon insceen, appear 2 bacon in SNB marchine
    public AudioClip collectSound;
    public ParticleSystem collectEffectPrefab;
    public string productType;
    private Collider2D col;
    public bool isCollected = false; // check if product is collected  

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogWarning("Missing Collider2D!");
    }

    private void OnMouseDown()
    {
        if (isCollected) return;
        Debug.Log("click " + productType);
        if (col == null || !col.enabled)
        {
            Debug.LogWarning("Cannot click, collider missing or disabled!");
            return;
        }
        Collect();
        // sound
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }


    void Collect()
    {
        Debug.Log("Collected");
        isCollected = true;

        if (collectEffectPrefab != null) //play effect when collect
        {
            ParticleSystem effect = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);  // effect disapear
        }
        Destroy(gameObject, collectSound != null ? collectSound.length : 0f); // product disapear
    }
}