using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = .5f;

    private Vector2 walkInput;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    public void OnWalk(InputAction.CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Debug.Log(transform.position);
    }

    private void Move()
    {
        // Initiating the input controls for up vertical and horizontal movement
        var deltaX = walkInput.x * Time.deltaTime * moveSpeed;
        var deltaY = walkInput.y * Time.deltaTime * moveSpeed;

        // Updating player's ship to be what the iput controls are showing, with clamps
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        // Moving player's ship to new input coordinates
        transform.position = new Vector2(newXPos, newYpos);
    }

    private void SetUpMoveBoundries()
    {
        // Labeling the main camera
        Camera gameCamera = Camera.main;

        // Restrict left and right movement of ship to stay on the screen
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        // Restrict up and down movement of ship to stay on the screen
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
