using UnityEngine;

public class WindArea : MonoBehaviour
{
    public Vector2 windForce = new Vector2(5f, 0f); // Initial wind force direction
    private float windChangeTimer = 0f; // Timer to change wind direction
    public float windChangeInterval = 3f; // Interval for wind direction change

    public Animator playerAnimator; // Reference to the player's Animator

    void Update()
    {
        windChangeTimer += Time.deltaTime;

        
        if (windChangeTimer >= windChangeInterval)
        {
            windForce = -windForce;
            windChangeTimer = 0f;

            FlipPlayerAnimation();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(windForce);

            if (playerAnimator == null)
                playerAnimator = other.GetComponent<Animator>();

            FlipPlayerAnimation();
        }
    }

    void FlipPlayerAnimation()
    {
        if (playerAnimator != null)
        {
            // Check the direction of the wind force
            bool isForceRight = windForce.x > 0;

            // Flip the player's animation by changing localScale
            Transform playerTransform = playerAnimator.transform;
            Vector3 scale = playerTransform.localScale;
            scale.x = isForceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            playerTransform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(windForce);

            // Cache the Animator reference and flip animation
            if (playerAnimator == null)
                playerAnimator = other.GetComponent<Animator>();

            FlipPlayerAnimation();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            // No additional actions needed when the player exits the wind area
        }
    }
}
