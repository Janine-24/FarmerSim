using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    public TextMeshProUGUI hintText;
    private GameObject currentDraggedObject;
    private ShopItem currentItem;
    public LayerMask obstacleLayer;
    public Transform habitatArea;
    public Bounds habitatBounds; // Store the bounds of the habitat area
    public LayerMask habitatLayer;
    private SpriteRenderer currentRenderer;
    private Collider2D currentCollider;



    public void Start()
    {
        hintText.gameObject.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
    }

    public void StartDragging(ShopItem item)
    {
        if (item.isAnimal && !HasAvailableHabitatForAnimal(item.animalType))
        {
            ShowHint("No available habitat for this animal.");
            Debug.Log("Start dragging failed: No available habitat.");
            return;
        }
        if (item.previewPrefab != null)
        {
            currentDraggedObject = Instantiate(item.previewPrefab);
        }
        else
        {
            currentDraggedObject = Instantiate(item.prefabToPlace);
        }
        if (currentDraggedObject.TryGetComponent(out SpriteRenderer renderer))
        {
            currentRenderer = renderer;
        }
        else
        {
            currentRenderer = null;
        }

        if (currentDraggedObject.TryGetComponent(out Collider2D collider))
        {
            currentCollider = collider;
            currentCollider.enabled = false;
        }
        else
        {
            currentCollider = null;
        }

        currentItem = item;
        Debug.Log("Start dragging: " + item.itemName);
    }

    private void Update()
    {
        if (currentDraggedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentDraggedObject.transform.position = mousePos;
            if (CanPlace(mousePos))
            {
                // can place when green
                SetObjectColor(Color.green);
            }
            else
            {
                // have obstacle, occur red colour
                SetObjectColor(Color.red);
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceItem();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CancelPlacement();
            }
        }
    }

    void CancelPlacement()
    {
        if (currentDraggedObject != null)
        {
            if (currentDraggedObject != null)
            {
                Destroy(currentDraggedObject);
                ShowHint("Placement cancelled.");

                currentDraggedObject = null;
                currentItem = null;
                currentRenderer = null;
                currentCollider = null;
            }
        }
    }


    void SetObjectColor(Color color)
    {
        if (currentRenderer != null)
        {
            if (currentItem != null && currentItem.isAnimal)
            {
                // if is animal, original colour
                currentRenderer.color = Color.white;
            }
            else
            {
                currentRenderer.color = color;
            }
        }
    }


    private void ShowHint(string message)
    {
        if (hintText == null)
            return;
        hintText.text = message;
        hintText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideHint));// Cancel any ongoing invocation of HideHint
        Invoke(nameof(HideHint), 2f); // occur in 2 second
    }
    private void HideHint()
    {
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void PlaceItem()
    {
        if (currentDraggedObject == null || currentItem == null)
            return;
        Vector2 placePos = currentDraggedObject.transform.position;

        if (CanPlace(placePos))
        {
            if (!PlayerCoinManager.Instance.HasEnoughCoins(currentItem.price))
            {
                ShowHint("Not enough coins to place.");
                Debug.Log("Placement failed: Not enough coins.");
                return;
            }
            if (currentItem.isAnimal && !HasAvailableHabitatForAnimal(currentItem.animalType))
            {
                ShowHint("No available habitat for this animal.");
                Debug.Log("Placement failed: No available habitat.");
                if (currentDraggedObject.TryGetComponent<Animal>(out var animal))
                {
                    currentRenderer = null;
                    currentCollider = null;
                    currentDraggedObject = null;
                    currentItem = null;

                    StartCoroutine(animal.DestroyIfUnassigned());
                }

                return;
            }

            PlayerCoinManager.Instance.SpendCoins(currentItem.price); //deduct coin
            GameObject placedAnimal = Instantiate(currentItem.prefabToPlace, placePos, Quaternion.identity); //place item
            if (placedAnimal.TryGetComponent<SpriteRenderer>(out var sr))
            {
                sr.color = Color.white;
            }
            //clean up drag
            Destroy(currentDraggedObject);
            currentDraggedObject = null;
            currentItem = null;
            currentRenderer = null;
            currentCollider = null;
            Debug.Log("Item placed and coins spent.");
        }
        else
        {
            Debug.Log("Cannot place here, have obstacles");
            ShowHint("Have obstacle");
        }
    }

    bool HasAvailableHabitatForAnimal(string animalType)
    {
        var habitats = Object.FindObjectsByType<Habitat>(FindObjectsSortMode.None);
        foreach (var h in habitats)
        {
            if (h.habitatType == animalType && h.animalsInHabitat.Count < h.maxanimals)
                return true;
        }
        return false;
    }

    bool CanPlace(Vector2 position)
    {
        if (currentRenderer == null) // if no have spriterenderer
            return false;  // dilarang letak dalam scene

        Vector2 size = currentRenderer.bounds.size;

        //obstacle layer
        Collider2D obstacleHit = Physics2D.OverlapBox(position, size, 0f, obstacleLayer);
        if (obstacleHit != null)
            return false;
        //habitat layer
        if (!currentItem.ignoreHabitatLayer)
        {
            Collider2D habitatHit = Physics2D.OverlapBox(position, size, 0f, habitatLayer);
            if (habitatHit != null)
                return false;
        }

        return true; //can place iff not hit with these layers
    }

    bool IsInHabitat(Vector2 position)
    {
        return habitatBounds.Contains(position);
    }
    private void OnDrawGizmos()
    {
        if (currentDraggedObject != null && currentRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(currentDraggedObject.transform.position, currentRenderer.bounds.size);
        }
    }
}