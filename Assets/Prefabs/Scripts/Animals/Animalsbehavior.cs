using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour
{
    public string animalType;
    private Habitat assignedHabitat;
    private Vector3 targetPosition;
    public float moveSpeed = 1.5f;
    public float waitTime = 2f;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;


    public void SetHabitat(Habitat habitat)
    {
        assignedHabitat = habitat;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }
    private void Start()
    {
        FindNearbyHabitat();
        StartCoroutine(WanderWithinHabitat());
    }
    private void Update()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement.x > 0.01f)
            spriteRenderer.flipX = true;
        else if (movement.x < -0.01f)
            spriteRenderer.flipX = false;
        lastPosition = transform.position;
    }
    private void FindNearbyHabitat()
    {
        var allHabitats = Object.FindObjectsByType<Habitat>(FindObjectsSortMode.None);
        Habitat nearest = null;
        float minDistance = float.MaxValue;

        foreach (var habitat in allHabitats)
        {
            if (habitat.CanAcceptAnimal(this))
            {
                float dist = Vector2.Distance(transform.position, habitat.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = habitat;
                }
            }
        }

        if (nearest != null)
        {
            nearest.RegisterAnimal(this);
        }
        else
        {
            Debug.LogWarning($"No suitable habitat found for {animalType}");
        }
    }

    private IEnumerator WanderWithinHabitat()
    {
        while (true)
        {
            if (assignedHabitat != null)
            {
                // 隨機在 habitat 範圍內挑一個點
                Bounds b = assignedHabitat.habitatBounds;
                targetPosition = new Vector3(
                    Random.Range(b.min.x, b.max.x),
                    Random.Range(b.min.y, b.max.y),
                    transform.position.z
                );

                // 走向該點
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        targetPosition,
                        moveSpeed * Time.deltaTime
                    );
                    yield return null;
                }

                // 等一下再繼續
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}
