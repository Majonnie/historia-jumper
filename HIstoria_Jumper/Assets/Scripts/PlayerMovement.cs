using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public float jumpForce; 

    public bool isJumping;
    public bool isGrounded;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;
    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;

    public static PlayerMovement instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;        // Création du mouvement horizontal

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
    }
    
    void FixedUpdate() 
    {
        MovePlayer(horizontalMovement);

        Flip(rb.velocity.x);
    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if(isJumping == true)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    void Flip(float _velocity)
    {
        if(_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        } else if(_velocity < 0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}
