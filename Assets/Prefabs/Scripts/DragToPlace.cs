using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private GameObject placedObject;

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
        currentItem = item;
        Debug.Log("Start dragging: " + item.itemName);

        if (item.previewPrefab == null)
        {
            Debug.LogWarning("Haven't setting previewPrefab，direct use prefabToPlace to show！");
            currentDraggedObject = Instantiate(item.prefabToPlace);
        }
        else
        {
            currentDraggedObject = Instantiate(item.previewPrefab); // 拖影预览
        }

        Collider2D col = currentDraggedObject.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    private void Update()
    {
        if (currentDraggedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentDraggedObject.transform.position = mousePos;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceItem();
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
        Vector2 placePos = currentDraggedObject.transform.position;

        if (CanPlace(placePos))
        {
            Instantiate(currentItem.prefabToPlace, placePos, Quaternion.identity);
            Destroy(currentDraggedObject);
            currentDraggedObject = null;
        }
        else
        {
            Debug.Log("Cannot place here, have obstacles");
            ShowHint("Have obstacle");
        }
    }
    bool CanPlace(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f, obstacleLayer);
        return colliders.Length == 0;
    }
    bool IsInHabitat(Vector2 position)
    {
        return habitatBounds.Contains(position);
    }

}
