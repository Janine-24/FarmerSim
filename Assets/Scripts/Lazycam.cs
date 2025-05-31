using UnityEngine;

public class Lazycam: MonoBehaviour
{
    private Animator animator;
    private Renderer rend;

    void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (rend.isVisible)
        {
            if (!animator.enabled)
                animator.enabled = true;
        }
        else
        {
            if (animator.enabled)
                animator.enabled = false;
        }
    }
}

