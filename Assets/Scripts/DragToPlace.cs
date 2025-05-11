using UnityEngine;

public class DragToPlace : MonoBehaviour
{
    public bool isAnimal = false;
    private bool placed = false;

    void Update()
    {
        if (placed) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;

        if (Input.GetMouseButtonUp(0))
        {
            if (IsInsideFarmArea(transform.position))
            {
                // detect is animal or not,if yes, add animations
                if (gameObject.CompareTag("Animal"))
                {
                    Animator anim = GetComponent<Animator>();
                    if (anim != null) anim.enabled = true;

                    if (GetComponent<AnimalBehavior>() == null)
                        gameObject.AddComponent<AnimalBehavior>();
                }

                Destroy(this); // stop drag
            }
            else
            {
                Destroy(gameObject); // if outside the habitat, it will be destroyed
            }
        }

    }

    bool IsInsideFarmArea(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        return hit != null && hit.CompareTag("FarmArea");

    }


}
