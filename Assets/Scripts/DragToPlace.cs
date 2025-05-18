using UnityEngine;
using UnityEngine.EventSystems;

public class DragToPlace : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToPlace; // Drag the prefab here in the Inspector
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.lossyScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Raycast to check if mouse is over the farm area
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("FarmArea"));

        if (hit.collider != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            if (prefabToPlace != null)
            {
                Instantiate(prefabToPlace, worldPosition, Quaternion.identity);
                Debug.Log("Placed successfully: " + prefabToPlace.name);
            }
            else
            {
                Debug.LogWarning("No prefab assigned!");
            }
        }
        else
        {
            Debug.Log("Placement failed: outside of farm area.");
        }

        Destroy(gameObject);
    }
}
