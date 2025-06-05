using UnityEngine;
using System.Collections; //use for IEnumerator

public class Animal : MonoBehaviour
{
    public string animalType; // e.g., "Cow", "Sheep"
    private Habitat assignedHabitat; // the habitat this animal is assigned to
    private Vector3 targetPosition; // the position the animal is moving towards
    public float moveSpeed = 1.5f; //speed movement
    public float waitTime = 2f; //animals wait time after reach target position
    private SpriteRenderer spriteRenderer; // to handle sprite flipping
    private Vector3 lastPosition; // to track last position for movement direction


    
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
        if (assignedHabitat != null)
            StartCoroutine(WanderWithinHabitat()); // start wandering if assigned habitat is found
        else
        {
            Debug.LogWarning($"{animalType} has no valid habitat and should not have been placed.");
            StartCoroutine(DestroyIfUnassigned());
        }
    }
    private void Update()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;// calculate velocity based on last position
        //flip the sprite based on movement direction
        if (velocity.x > 0.05f)
            spriteRenderer.flipX = true;
        else if (velocity.x < -0.05f)
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
            assignedHabitat = nearest;
        }

        else
        {
            Debug.LogWarning($"No suitable habitat found for {animalType}, will destroy after delay.");
        }
    }

    public IEnumerator DestroyIfUnassigned()
    {
        float timeout = 5f; // destroy after 5 sec
        float timer = 0f;
        //prevent sprite renderer null
        if (spriteRenderer == null)
        {
            yield break;
        }

        Color originalColor = spriteRenderer.color;

        while (assignedHabitat == null && timer < timeout)
        {
            if (this == null || spriteRenderer == null)
            {
                yield break;
            }

            //let animals half transparent and disapper
            float alpha = Mathf.Lerp(1f, 0.2f, timer / timeout);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        if (assignedHabitat == null)
        {
            Debug.Log($"{animalType} destroyed due to no habitat.");
            Destroy(gameObject);
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

    private void OnDestroy()
    {
        // prevent occur mistake calculation about maxanial in habitat
        if (assignedHabitat != null)
        {
            assignedHabitat.UnregisterAnimal(this);
        }

    }
}