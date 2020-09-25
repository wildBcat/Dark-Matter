using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] GameObject[] waves = default;

    [SerializeField] GameObject player = default;

    [Tooltip("Sets a delay between planet spawn times. The higher the number, the longer the time between spawns.")]
    [SerializeField] float waveSpawnTime = default;

    [Tooltip("Sets a randomizer to the planet spwan time. The generated time will either decrees or increase the spwan time.")]
    [SerializeField] float waveTimeRandomizer = default;

    [Tooltip("Sets the time delay before spawing the first planet.")]
    [SerializeField] int waveBeginSpawnTime = default;

    // A list that stores the planets
    readonly List<GameObject> waveList = new List<GameObject>();

    // Variable for the instantiated planet
    private GameObject newWave = default;

    ScreenBounds screenBounds = default;

    // Sets the height above the screen view to spawn the celestial bodies 
    readonly int padding = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the vaslue from screenBounds from the ScreenBounds script
        screenBounds = gameObject.AddComponent<ScreenBounds>();

        StartCoroutine(createWaves());
    }

    IEnumerator createWaves()
    {
        // Creates a new list, based of the array number
        for (int i = 0; i < waves.Length; i++)
        {
            waveList.Add(waves[i]);
        }
        yield return new WaitForSeconds(waveBeginSpawnTime);

        Debug.Log(waveList.Count);

        for (int j = 0; j < waves.Length; j++)
        {
            // Chooses a random object from the list, generates it, and then deletes it from the list
            int randomIndex = Random.Range(0, waveList.Count);

            newWave = Instantiate(waveList[randomIndex],
                new Vector3(Random.Range(player.transform.position.x - 3, player.transform.position.x + 3), screenBounds.yMax + padding, 0),
                Quaternion.Euler(0, 0, 0));

            waveList.RemoveAt(randomIndex);

            //if the list decreased to zero, reinstall it
            if (waveList.Count == 0)
            {
                for (int i = 0; i < waves.Length; i++)
                {
                    waveList.Add(waves[i]);
                }
            }

            yield return new WaitForSeconds(waveSpawnTime + Random.Range(-waveTimeRandomizer, waveTimeRandomizer));
        }
    }
}
