using UnityEngine;

//"Cute Fantasy asset pack" by Kenmi ¨C Free Version License
//https://kenmi-art.itch.io/cute-fantasy-rpg
//Used with permission for non-commercial purposes. Modified assets not redistributed.

//player movement
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  //moving speed
    private Rigidbody2D rb;       //collide
    private Vector2 movement;
    private Animator animator;    //animation
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //get sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //update animator parameters
        animator.SetFloat("MoveX",movement.x);
        animator.SetFloat("MoveY",movement.y);
        animator.SetBool("IsMoving", movement != Vector2.zero);

        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with" + collision.gameObject.name);
    }
}
