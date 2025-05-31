using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            Item item = GetComponent<Item>();

            if (item == null)
            {
                Debug.LogError("Item component is missing!");
            }
            else if (item.data == null)
            {
                Debug.LogError($"ItemData is NULL on object: {gameObject.name}");
            }
            else
            {
                player.inventoryManager.Add("backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}

