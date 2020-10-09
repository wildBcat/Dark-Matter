using UnityEngine;

/// <summary>
/// sets up player movement, speed, and boundries 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    ScreenBounds screenBounds = default;

    [Tooltip("Sets the player's speed. The higher the number, the faster the movement.")]
    [SerializeField] float moveSpeed = default;

    [Tooltip("Sets the barrier between the player and the edge of the screen. The higher the number, the farther the distance between the two.")]
    [SerializeField] float padding = default;

    private Rigidbody2D rb = default;
    private SpriteRenderer sr = default;
    private Animator animator;
    private Vector2 inputVector = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = gameObject.AddComponent<ScreenBounds>();
    }

    // Gets input System Listeners
    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /*
     * Assignes the input control value (multiplied by the speed value) to the delta x/y values
     * Assignes the player ship's position (along with confinement boundries) to the new x/y positions
     * Adds force to the player ship's rigid body equal to the values of delta x/y
     * Updates the player ship's position to stsay within  the boundries
     * Assigns the animators for the player ship, dependent on position and speed, as long as the ship is alive
     * Flips the animation x orientation for left/right movement to represent left and right banking of ship, as long as the ship is alive
     */
    private void Move()
    {
        var deltaX = inputVector.x * moveSpeed;
        var deltaY = inputVector.y * moveSpeed;
        float newYpos = Mathf.Clamp(transform.position.y, screenBounds.yMin + padding, screenBounds.yMax - padding);
        var newXPos = Mathf.Clamp(transform.position.x, screenBounds.xMin + padding, screenBounds.xMax - padding);
        rb.AddForce(new Vector2(deltaX, deltaY));
        transform.position = new Vector2(newXPos, newYpos);
        if(animator != null)
        {
            animator.SetFloat("Horizontal", inputVector.x);
            animator.SetFloat("Vertical", inputVector.y);
            animator.SetFloat("Speed", inputVector.sqrMagnitude);
        }

        if (sr != null)
        {
            if (inputVector.x > 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }
}
