using UnityEngine;

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    public SimpleCameraFollow(Transform target)
    {
        this.target = target;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = new Vector3(target.position.x, target.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
        }
    }
}
public class camera : MonoBehaviour
{ 
    
        public Transform target;
        public float smoothSpeed = 0.125f;
        public Vector3 offset;
        public Vector2 minBounds;
        public Vector2 maxBounds;

        void LateUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // limit cam moving area
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);

            transform.position = smoothedPosition;
        }
 }
// ObjectVisibility.cs
public class ObjectVisibility : MonoBehaviour
{
    public float activationDistance = 15f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        gameObject.SetActive(dist < activationDistance);
    }
}

