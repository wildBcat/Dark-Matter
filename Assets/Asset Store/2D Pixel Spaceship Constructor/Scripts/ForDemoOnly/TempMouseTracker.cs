using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMouseTracker : MonoBehaviour
{
    Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(targetPos.x, targetPos.y, 0.0f);
    }
}
