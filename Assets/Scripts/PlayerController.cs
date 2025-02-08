using UnityEngine;
using UnityEngine.UI; // Add this for using UI elements
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 10f; // Base jump force
    public float maxChargeTime = 1f; // Maximum time to charge a jump
    public float moveSpeed = 2f; // Horizontal movement speed
    public float moveCooldown = 0.15f; // Cooldown time after landing before allowing horizontal movement
    public float minChargeTime = 0.1f; // Minimum time required to charge the jump
    public float slideSpeed = 5f; // Speed at which the player slides on angled surfaces
    public float slopeLimit = 45f; // Maximum angle for the player to slide on
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float jumpCharge;
    private bool isCharging;
    private float moveCooldownTimer;
    private bool wasGroundedLastFrame;
    private bool isCooldownActive;

    private readonly float[] jumpLevels = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };

    private SoundEffects audioManager;

    [Header("Ground Detection")]
    public float raycastDistance = 1f; // Length of the raycasts
    public float raycastOffsetX = 0.5f; // How far left/right the raycasts should be
    public float raycastYOffset = 0.1f; // Y-offset for raycasts

    [Header("Jump Counter")]
    public Text jumpCountText; // UI Text to display the jump count
    private int jumpCount = 0; // Tracks the number of jumps

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = GetComponent<SoundEffects>();
    }

    void Update()
    {
        HandleGroundCheck(); // Updated with raycasting from left and right
        HandleJumpCharge();
        HandleHorizontalMovement();
        UpdateAnimator();
        HandleSliding(); // Added sliding logic
    }

    // Updated ground detection using two raycasts (left and right)
    void HandleGroundCheck()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down, raycastDistance, groundLayer);

        isGrounded = hitLeft.collider != null || hitRight.collider != null;
    }

    void HandleJumpCharge()
    {
        if (isCooldownActive) return;

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (!isCharging)
            {
                isCharging = true;
                jumpCharge = 0f;
            }

            jumpCharge += Time.deltaTime;
            jumpCharge = Mathf.Clamp(jumpCharge, 0, maxChargeTime);
            animator.SetBool("isCharging", true);
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isCharging)
            {
                PerformJump();
            }
            isCharging = false;
            animator.SetBool("isCharging", false);
        }
    }

    void PerformJump()
    {
        float selectedJumpLevel = jumpLevels[0];
        foreach (float level in jumpLevels)
        {
            if (jumpCharge <= level)
            {
                selectedJumpLevel = level;
                break;
            }
        }

        float calculatedJumpForce = jumpForce * (selectedJumpLevel / maxChargeTime);
        rb.velocity = new Vector2(rb.velocity.x, calculatedJumpForce);
        animator.SetBool("isJumping", true);
        audioManager.PlayJumpSound();


        jumpCount++;
        if (jumpCountText != null)
        {
            jumpCountText.text = "Jumps: " + jumpCount;
        }
    }

    void HandleHorizontalMovement()
    {
        if (moveCooldownTimer > 0)
        {
            moveCooldownTimer -= Time.deltaTime;
            return;
        }

        if (isGrounded && !Input.GetKey(KeyCode.Space))
        {
            float moveInput = Input.GetAxis("Horizontal");

            if (Mathf.Abs(moveInput) > 0)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
                animator.SetBool("isRunning", true);
                audioManager.PlayRunSound();
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("isRunning", false);
            }
        }
        else if (!isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            animator.SetBool("isRunning", false);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void HandleSliding()
    {
        if (isGrounded)
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down, raycastDistance, groundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down, raycastDistance, groundLayer);

            RaycastHit2D hit = hitLeft.collider != null ? hitLeft : hitRight;

            if (hit.collider != null)
            {
                float angle = Vector2.Angle(hit.normal, Vector2.up);

                if (angle > slopeLimit)
                {

                    Vector2 slideDirection = new Vector2(-hit.normal.y, hit.normal.x).normalized; 
                    rb.velocity = new Vector2(slideDirection.x * slideSpeed, rb.velocity.y); 

                    animator.SetBool("isSliding", true);

                    animator.SetBool("isRunning", false);
                }
                else
                {
                    
                    animator.SetBool("isSliding", false);
                }
            }
        }
        else
        {
            animator.SetBool("isSliding", false);
        }
    }

    void FixedUpdate()
    {
        double inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (!wasGroundedLastFrame && isGrounded)
        {
            moveCooldownTimer = moveCooldown;
            isCooldownActive = true;
            Invoke(nameof(ResetCooldown), moveCooldown);
            rb.rotation = 0f;
            animator.SetBool("isJumping", false);
            audioManager.PlayLandSound();
        }
        wasGroundedLastFrame = isGrounded;
        rb.freezeRotation = true;

        if (inputHorizontal > 0)
        {
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        if (inputHorizontal < 0)
        {
            gameObject.transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
        }
    }

    void ResetCooldown()
    {
        isCooldownActive = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x - raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down * raycastDistance); // Left ray
        Gizmos.DrawRay(new Vector2(transform.position.x + raycastOffsetX, transform.position.y + raycastYOffset), Vector2.down * raycastDistance); // Right ray
    }

    void UpdateAnimator()
    {
        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        animator.SetBool("isJumping", !isGrounded);

        if (isCharging)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isCharging", true);
        }
        else if (isGrounded && Mathf.Abs(rb.velocity.x) > 0)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isCharging", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isCharging", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SceneTrigger"))
        {
            SceneManager.LoadScene(2);
        }
    }
    
    
}
