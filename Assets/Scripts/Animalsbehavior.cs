using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveRadius = 2f;
    private Vector3 origin;
    private Vector3 target;
    private bool isMoving = false;

    void Start()
    {
        origin = transform.position;
        PickNewTarget();
        InvokeRepeating(nameof(PickNewTarget), 3f, 5f); // new behavior per 5 second
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target) > 0.1f)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

    void PickNewTarget()
    {
        float offsetX = Random.Range(-moveRadius, moveRadius);
        float offsetY = Random.Range(-moveRadius, moveRadius);
        target = origin + new Vector3(offsetX, offsetY, 0);
    }
}
