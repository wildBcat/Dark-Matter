using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Sets the player's speed. The higher the number, the faster the movement.")]
    [SerializeField] float moveSpeed = default;

    [Tooltip("Sets the barrier between the player and the edge of the screen. The higher the number, the farther the distance between the two.")]
    [SerializeField] float padding = default;

    //Input System Variables
    private Vector2 walkInput = default;
    private Vector2 dragInput = default;
    //private Vector2 touchInput;
    private float clickInput = default;

    ScreenBounds screenBounds = default;

    private Rigidbody2D rb = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = gameObject.AddComponent<ScreenBounds>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveWithMouse();
        Touch();
        Move();
    }

    // Input System Listeners
    public void Keyboard(InputAction.CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>();
    }

    public void Mouse(InputAction.CallbackContext context)
    {
        dragInput = context.ReadValue<Vector2>();
    }

    //public void Touch(InputAction.CallbackContext context)
    //{
    //    touchInput = context.ReadValue<Vector2>();
    //}

    public void Enable(InputAction.CallbackContext context)
    {
        clickInput = context.ReadValue<float>();
    }

    private void Move()
    {
        // Initiating the input controls for up vertical and horizontal movement
        var deltaX = walkInput.x * moveSpeed;
        var deltaY = walkInput.y * moveSpeed;

        // Updating player's ship to be what the iput controls are showing, with clamps
        float newYpos = Mathf.Clamp(transform.position.y, screenBounds.yMin + padding, screenBounds.yMax - padding);
        var newXPos = Mathf.Clamp(transform.position.x, screenBounds.xMin + padding, screenBounds.xMax - padding);
        
        // Moving player's ship to new input coordinates
        //transform.position = new Vector2(newXPos, newYpos);
        rb.AddForce(new Vector2(deltaX, deltaY));
        transform.position = new Vector2(newXPos, newYpos);
    }

    private void MoveWithMouse()
    {
        if (clickInput > 0)
        {
            var screenPoint = new Vector3(dragInput.x, dragInput.y, 0);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(screenPoint);
            mousePosition.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Touch()
    {
        if(Input.touchCount > 0)
        {
            // Need to add something here
        }
    }
}
