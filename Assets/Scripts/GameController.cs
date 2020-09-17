using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    EdgeCollider2D edgeCollider;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { bottomLeft,
        new Vector2(bottomLeft.x, topRight.y),
        topRight,
        new Vector2(topRight.x, bottomLeft.y),
        bottomLeft
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
