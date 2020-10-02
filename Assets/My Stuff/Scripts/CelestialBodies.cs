using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodies : MonoBehaviour
{
    ScreenBounds screenBounds = default;

    [Header("Planet Settings")]
    [Tooltip("Sets planet prefabs. The larger size number, the more planet prefabs you can set.")]
    [SerializeField] GameObject[] planets = default;
    [Tooltip("Sets a delay between planet spawn times. The higher the number, the longer the time between spawns.")]
    [SerializeField] float planetSpawnTime = default;
    [Tooltip("Sets a randomizer to the planet spwan time. The generated time will either decrees or increase the spwan time.")]
    [SerializeField] float planetTimeRandomizer = default;
    [Tooltip("Sets the time delay before spawing the first planet.")]
    [SerializeField] int planetBeginSpawnTime = default;


    [Header("Asteroid Settings")]
    [Tooltip("Sets asteroid prefabs. The larger size number, the more asteroid prefabs you can set.")]
    [SerializeField] GameObject[] asteroids = default;
    [Tooltip("Sets a delay between asteroid spawn times. The higher the number, the longer the time between spawns.")]
    [SerializeField] float asteroidSpawnTime = default;
    [Tooltip("Sets a randomizer to the asteroid spwan time. The generated time will either decrees or increase the spwan time.")]
    [SerializeField] float asteroidTimeRandomizer = default;
    [Tooltip("Sets the time delay before spawing the first asteroid.")]
    [SerializeField] int asteroidBeginSpawnTime = default;

    [Header("Starfield Settings")]
    [Tooltip("Sets starfield prefabs. The larger size number, the more starfield prefabs you can set.")]
    [SerializeField] GameObject[] starfield = default;
    [Tooltip("Sets a delay between starfield spawn times. The higher the number, the longer the time between spawns.")]
    [SerializeField] float starfieldSpawnTime = default;
    [Tooltip("Sets a randomizer to the starfield spwan time. The generated time will either decrees or increase the spwan time.")]
    [SerializeField] float starfieldTimeRandomizer = default;
    [Tooltip("Sets the time delay before spawing the first starfield.")]
    [SerializeField] int starfieldBeginSpawnTime = default;

    // Variable for the instantiated planet
    private GameObject newPlanet = default;

    // Variable for the instantiated asteroid
    private GameObject newAstoid = default;

    // Variable for the instantiated starfield
    private GameObject newStarfield = default;

    // A list that stores the planets
    readonly List<GameObject> celestialBodyList = new List<GameObject>();

    // A list that stores the asteroids
    readonly List<GameObject> astroidList = new List<GameObject>();

    // A list that stores the starfields
    readonly List<GameObject> starfieldList = new List<GameObject>();

    // Sets the height above the screen view to spawn the celestial bodies 
    private readonly float padding = 1.5f;

    private void Start()
    {
        // Sets the vaslue from screenBounds from the ScreenBounds script
        screenBounds = gameObject.AddComponent<ScreenBounds>();

        StartCoroutine(PlanetCreation());

        StartCoroutine(AsteroidCreation());

        StartCoroutine(StarfieldCreation());
    }

    IEnumerator PlanetCreation()
    {
        // Creates a new list, based of the array number
        for (int i = 0; i < planets.Length; i++)
        {
            celestialBodyList.Add(planets[i]);
        }
        yield return new WaitForSeconds(planetBeginSpawnTime);
        while (true)
        {
            // Chooses a random object from the list, generates it, and then deletes it from the list
            int randomIndex = Random.Range(0, celestialBodyList.Count);

            newPlanet = Instantiate(celestialBodyList[randomIndex],
                new Vector3(Random.Range(screenBounds.xMin, screenBounds.xMax), screenBounds.yMax + padding, 0),
                Quaternion.Euler(0, 0, 0));

            celestialBodyList.RemoveAt(randomIndex);

            //if the list decreased to zero, reinstall it
            if (celestialBodyList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    celestialBodyList.Add(planets[i]);
                }
            }

            yield return new WaitForSeconds(planetSpawnTime + Random.Range(-planetTimeRandomizer, planetTimeRandomizer));
        }
    }

    IEnumerator AsteroidCreation()
    {
        // Creates a new list, based of the array number
        for (int i = 0; i < asteroids.Length; i++)
        {
            astroidList.Add(asteroids[i]);
        }
        yield return new WaitForSeconds(asteroidBeginSpawnTime);
        while (true)
        {
            // Chooses a random object from the list, generates it, and then deletes it from the list
            int randomIndex = Random.Range(0, astroidList.Count);

            newAstoid = Instantiate(astroidList[randomIndex],
                new Vector3(Random.Range(screenBounds.xMin, screenBounds.xMax), screenBounds.yMax + padding, 0),
                Quaternion.Euler(0, 0, 0));

            astroidList.RemoveAt(randomIndex);

            //if the list decreased to zero, reinstall it
            if (astroidList.Count == 0)
            {
                for (int i = 0; i < asteroids.Length; i++)
                {
                    astroidList.Add(asteroids[i]);
                }
            }

            yield return new WaitForSeconds(asteroidSpawnTime + Random.Range(-asteroidTimeRandomizer, asteroidTimeRandomizer));
        }
    }

    IEnumerator StarfieldCreation()
    {
        // Creates a new list, based of the array number
        for (int i = 0; i < starfield.Length; i++)
        {
            starfieldList.Add(starfield[i]);
        }
        yield return new WaitForSeconds(starfieldBeginSpawnTime);
        while (true)
        {
            // Chooses a random object from the list, generates it, and then deletes it from the list
            int randomIndex = Random.Range(0, starfieldList.Count);

            newStarfield = Instantiate(starfieldList[randomIndex],
                new Vector3(Random.Range(screenBounds.xMin, screenBounds.xMax), screenBounds.yMax + padding, 0),
                Quaternion.Euler(0, 0, 0));

            starfieldList.RemoveAt(randomIndex);

            //if the list decreased to zero, reinstall it
            if (starfieldList.Count == 0)
            {
                for (int i = 0; i < starfield.Length; i++)
                {
                    starfieldList.Add(starfield[i]);
                }
            }

            yield return new WaitForSeconds(starfieldSpawnTime + Random.Range(-starfieldTimeRandomizer, starfieldTimeRandomizer));
        }
    }
}