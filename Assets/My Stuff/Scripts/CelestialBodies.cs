using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets up the type fo celestial bodies, stores them in lists, and randomly instantiates them withing the screen limits
/// </summary>
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

    [Header("Starfield Settings")]
    [Tooltip("Sets starfield prefabs. The larger size number, the more starfield prefabs you can set.")]
    [SerializeField] GameObject[] starfield = default;
    [Tooltip("Sets a delay between starfield spawn times. The higher the number, the longer the time between spawns.")]
    [SerializeField] float starfieldSpawnTime = default;
    [Tooltip("Sets a randomizer to the starfield spwan time. The generated time will either decrees or increase the spwan time.")]
    [SerializeField] float starfieldTimeRandomizer = default;
    [Tooltip("Sets the time delay before spawing the first starfield.")]
    [SerializeField] private int starfieldBeginSpawnTime = default;

    // A list that stores the planets
    readonly List<GameObject> celestialBodyList = new List<GameObject>();

    // A list that stores the starfields
    readonly List<GameObject> starfieldList = new List<GameObject>();

    // Sets the height above the screen view to spawn the celestial bodies 
    private readonly float padding = 8f;

    private void Start()
    {
        screenBounds = gameObject.AddComponent<ScreenBounds>();
        StartCoroutine(PlanetCreation());
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
            _ = Instantiate(celestialBodyList[randomIndex],
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
            _ = Instantiate(starfieldList[randomIndex],
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