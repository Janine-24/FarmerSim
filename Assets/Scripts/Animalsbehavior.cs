using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveRadius = 2f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = GetRandomPosition();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);

        // move
        if (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (animator != null) animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator != null) animator.SetBool("isWalking", false);
            targetPosition = GetRandomPosition();
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector2 offset = Random.insideUnitCircle * moveRadius;
        return startPosition + new Vector3(offset.x, offset.y, 0);
    }
}
