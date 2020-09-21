using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class Scrolling : MonoBehaviour
{
    [Tooltip("Sets the scroll speed of the background. Negative numbers move background down, and positive numbers move background up. " +
    "The higher the number in either direction, the faster the speed.")]
    public float scrollSpeed = -0.03f;

    //moving the object with the defined speed
    private void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}
