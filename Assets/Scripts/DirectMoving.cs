using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class DirectMoving : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Scroll speed of the background. Negative numbers move background down, and positive numbers move background up. " +
    "The higher the number in each direction, the faster the speed.")]
    [SerializeField] float speed = -0.03f;

    //moving the object with the defined speed
    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
