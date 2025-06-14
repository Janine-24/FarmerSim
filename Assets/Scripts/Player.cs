using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;


    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
    }

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
       

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Machine"))
            {
                var machine = hit.collider.GetComponent<ProductionMachine>();
                machine?.Interact();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            Inventory toolbar = inventoryManager.toolbar;
            Inventory.Slot selectedSlot = toolbar.selectedSlot;

            if (selectedSlot != null && selectedSlot.count > 0 && selectedSlot.itemName == "Shovel3")
            {
                if (tileManager.IsInteractable(position))
                {
                    Debug.Log("Tile is interactable");
                    tileManager.SetInteracted(position);
                }
                else
                {
                    tileManager.ReverseInteracted(position);
                }

                Debug.Log("Used shovel: " + selectedSlot.itemName);

                // 👇 Remove one from the stack
                selectedSlot.count--;
                // If item count is 0, clear the slot
                if (selectedSlot.count <= 0)
                {
                    toolbar.selectedSlot = null;

                    inventoryManager.UpdateToolbarUI();
                }
                // Update UI
                inventoryManager.UpdateToolbarUI();

                // 👇 If no more left, clear the slot
                if (selectedSlot.count <= 0)
                {
                    toolbar.selectedSlot = null;
                    inventoryManager.UpdateToolbarUI();
                }

                GameManager.instance.uiManager.RefreshAll();
                inventoryManager.toolbarUI.SelectSlot(inventoryManager.toolbarUI.GetSelectedSlotIndex());
            }
            else
            {
                Debug.Log("No shovel selected or item count is 0.");
            }
        }


        if (Input.GetKeyDown(KeyCode.G)) // 或改成靠近自动触发
        {
            Vector2 pos = transform.position;
            Collider2D[] testHits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
            foreach (var hit in testHits)
            {
                Debug.Log($"📦 Physics2D Hit: {hit.name}");
            }

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<PlantInstance>(out var plant) && plant.CanBeWatered)
                {
                    PlantWateringUI.Instance.OpenPanel(plant);
                    break;
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if (GameManager.instance.tileManager.IsPlantable(position) && GameManager.instance.tileManager.NoPlantOnTile(position))
            {
                Inventory toolbar = inventoryManager.toolbar;

                Inventory.Slot selectedSlot = toolbar.selectedSlot;


                if(selectedSlot != null && selectedSlot.count > 0 && selectedSlot.itemData != null && selectedSlot.itemData.itemType == ItemType.Seed)
                {
                    Debug.Log("The itemType selected is " +  selectedSlot.itemData.itemType);
                    Debug.Log("Planted: " + selectedSlot.itemName);
                    GameManager.instance.tileManager.ShowPlantingCue(position);

                    tileManager.PlantSeed(position, selectedSlot.itemData);

                    LevelSystem.Instance.AddXP(10);//level experience add 



                    selectedSlot.count--;

                    // Update the UI
                    inventoryManager.UpdateToolbarUI();

                    // If item count is 0, clear the slot
                    if (selectedSlot.count <= 0)
                    {
                        toolbar.selectedSlot = null;

                        inventoryManager.UpdateToolbarUI();
                    }
                }
            
                    else
                    {
                        if (selectedSlot != null)
                        {
                            if (selectedSlot.itemData != null)
                            {
                                Debug.Log("The itemType selected is " + selectedSlot.itemData.itemType);
                            }
                            else
                            {
                                Debug.Log("itemData is null");
                            }
                        }
                        else
                        {
                            Debug.Log("Selected slot is null");
                        }

                        Debug.Log("No seeds selected or empty slot.");
                    }

           
            }
            else
            {
                Debug.Log("Tile is not plantable");
            }
        }

    }

    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}