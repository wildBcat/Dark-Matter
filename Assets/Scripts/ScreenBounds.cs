﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    Camera mainCamera;

    // Boundry coordinate variables
    [HideInInspector]
    public float xMin = default;
    [HideInInspector]
    public float xMax = default;
    [HideInInspector]
    public float yMin = default;
    [HideInInspector]
    public float yMax = default;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        xMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        yMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }
}
