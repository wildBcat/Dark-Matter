using UnityEngine;

/// <summary>
/// This script defines the size of the ‘Boundary’ depending on Viewport. When objects go beyond the ‘Boundary’, they are destroyed or deactivated.
/// </summary>
public class Boundary : MonoBehaviour
{
    private BoxCollider2D boundareCollider;

    private void Start()
    {
        boundareCollider = GetComponent<BoxCollider2D>();
        ResizeCollider();
    }

    // Sets the boundry collider's size to match the viewport's size
    void ResizeCollider()
    {
        Vector2 viewportSize = Camera.main.ViewportToWorldPoint(new Vector2(1f, 1)) * 2;
        boundareCollider.size = viewportSize;
    }

    // Sets destroy commands for any object exiting the boundry collider that has a matching tag
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Planet"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Starfield"))
        {
            Destroy(collision.gameObject);
        }
    }
}
