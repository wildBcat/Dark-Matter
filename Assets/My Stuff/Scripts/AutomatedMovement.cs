using UnityEngine;

/// <summary>
/// Moves the attached object along the Y-axis at the defined speed
/// </summary>
public class AutomatedMovement : MonoBehaviour
{
    [SerializeField] float speed = default;

    //moving the object with the defined speed
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
