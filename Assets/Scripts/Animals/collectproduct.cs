using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectproduct : MonoBehaviour
{
    public int rewardAmount = 1; // set the number of product collect like collect 1 bacon insceen, appear 2 bacon in SNB marchine
    public AudioClip collectSound;
    public ParticleSystem collectEffectPrefab;
    public string productType;
    private Collider2D col;
    public bool isCollected = false; // check if product is collected  
    public Item item; // 用于添加到背包
    private Player player;
    public LayerMask ClickableLayer; // Layer for clickable objects, set in inspector

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogWarning("Missing Collider2D!");

        player = FindObjectOfType<Player>();

        if (item == null)
        {
            item = GetComponent<Item>();
            if (item == null)
            {
                Debug.LogWarning("Collectproduct missing Item reference.");
            }
        }
    }

    public void InitializeClick()
    {
        col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            col.enabled = true; // refresh forced
        }
        Debug.Log("✅ Product click system initialized");
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

        if (item == null)
            Debug.LogError("❌ item IS NULL AT CLICK TIME!");

        Collect();
        // sound
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }


    void Collect()
    {
        Debug.Log("Collected");
        isCollected = true;

        if (item != null && player != null)
        {
            player.inventoryManager.Add("backpack", item); // ✅ Pass the full Item
        }
        else
        {
            Debug.LogWarning("Missing item or player.");
        }
        if (player == null)
            Debug.LogError("❌ Player is NULL");
        if (item == null)
            Debug.LogError("❌ Item is NULL");

        if (collectEffectPrefab != null) //play effect when collect
        {
            ParticleSystem effect = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            effect.Play();

            Destroy(effect.gameObject, effect.main.duration);  // effect disapear
        }
        Destroy(gameObject, collectSound != null ? collectSound.length : 0f); // product disapear
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get mouse position in game
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, ClickableLayer);

            if (hit.collider != null) // check if hit a product
            {
                var collect = hit.collider.GetComponent<Collectproduct>();
                if (collect != null && !collect.isCollected)
                {
                    collect.SendMessage("OnMouseDown"); 
                }
            }
        }
    }
}