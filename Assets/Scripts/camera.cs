using UnityEngine;

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

