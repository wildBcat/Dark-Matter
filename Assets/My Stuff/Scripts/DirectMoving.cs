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
        if (gameObject.CompareTag("Asteroid"))
        {
            Asteroids();
        }
        else if (gameObject.CompareTag("Starfield"))
        {
            Asteroids();
        }
        else if (gameObject.CompareTag("Planet"))
        {
            Planets();
        }
    }

    private void Planets()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    private void Asteroids()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(0, 0, 0));
    }
}
