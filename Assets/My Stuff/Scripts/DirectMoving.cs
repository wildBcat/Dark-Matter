using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class DirectMoving : MonoBehaviour
{
    CelestialBodies celestialBodies = default;
    [SerializeField] int speed = default;

    private void Start()
    {
        celestialBodies = FindObjectOfType<CelestialBodies>();
    }

    //moving the object with the defined speed
    private void Update()
    {
        if (gameObject.CompareTag("Asteroid"))
        {
            Asteroids();
        }
        else
        {
            Planets();
        }
    }

    private void Planets()
    {
        transform.Translate(Vector3.up * celestialBodies.planetSpeed * Time.deltaTime, Space.World);
    }

    private void Asteroids()
    {
        transform.Translate(Vector3.up * celestialBodies.asteroidSpeed * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
    }
}
