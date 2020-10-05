using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class DirectMoving : MonoBehaviour
{
    [SerializeField] float speed = default;

    //moving the object with the defined speed
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
