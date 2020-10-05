using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class EnemySpawner : MonoBehaviour
{
    [Header("Waves")]
    [Tooltip("Sets the waves that will be in the level.")]
    [SerializeField] List<WaveConfig> waveList = default;

    [Header("Wave Spawning Settings")]
    [Tooltip("Sets wheter or not you want a wait between waves. Checked is yes, unchecked is no.")]
    [SerializeField] private bool waveInterval = default;
    [Tooltip("Sets the interval time between wave spawns. " +
        "The higher the number, the longer the wait time before the next wave spawns.")]
    [SerializeField] private float waveIntervalTime = default;
    [Tooltip("Sets a variability for the wave interval. " +
        "Whatever number is set, the wave interval will be randomly increased or decreased " +
        "by a value randomly picked between 0 and the chosen number. " +
        "You must choose a number lower than the wave interval time, or this number will become equal to the wave interval time at start.")]
    [SerializeField] private float waveIntervalVariability = default;
    [Tooltip("Sets whether of not to loop the wave. Setting it to true will continually spawn enemies on the wave." +
        "Setting it to false will only spawn the enemy wave once.")]
    [SerializeField] private bool looping = false;

    // Keeps track of the number of paths in a wave whose enemies have finished spawning
    int pathsFinished = 0;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (waveIntervalVariability > waveIntervalTime)
        {
            waveIntervalVariability = waveIntervalTime;
        }

        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping); // Need to fix this
    }

    /* 
     Sets the current wave number and starts the wave creation coroutine. 
     Starts each new wave after the first one has processed, following a wave interval wait time.
    */
    private IEnumerator SpawnAllWaves() 
    {        
        for (int waveIndex = 0; waveIndex < waveList.Count; waveIndex++)
        {
            pathsFinished = 0;
            WaveConfig currentWave = waveList[waveIndex];

            yield return StartCoroutine(SpawnAllPathsInWave(currentWave));
            if (waveInterval == true)
            {
                yield return new WaitForSeconds(waveIntervalTime + Random.Range(-waveIntervalVariability, waveIntervalVariability));
            }
        }
    }

    /* 
     Sets the current path number paths the first and starts the path creation coroutine. 
     Starts each new path after the first one has processed.
     It does not allow to move back up into the SpawnAllWaves coroutine until all enemies in all paths of the current wave have spawned
    */
    private IEnumerator SpawnAllPathsInWave(WaveConfig waveConfig)
    {
        int pathIndex;
        for (pathIndex = 0; pathIndex < waveConfig.prefabs.Length; pathIndex++)
        {
            WaveConfig.Prefabs currentPath = waveConfig.prefabs[pathIndex];

            StartCoroutine(SpawnAllPaths(currentPath));
        }

        yield return new WaitUntil(() => pathsFinished == waveConfig.prefabs.Length);

        //while (pathsFinished != waveConfig.prefabs.Length)
        //{
        //    yield return null;
        //}
    }

    /* 
     Sets the values for each serialized field in the wave config and begins spawning enemies. 
     Adds a count to the pathsFinished variable after all enemies on the path have been spawned.
    */
    private IEnumerator SpawnAllPaths(WaveConfig.Prefabs currentPath)
    {
        PathCreator pathPrefab = currentPath.pathPrefab;
        PathFollower enemyPrefab = currentPath.enemyPrefab;
        float timeBetweenSpawns = currentPath.timeBetweenSpawns;
        int numberOfEnemies = currentPath.numberOfEnemies;
        float moveSpeed = currentPath.moveSpeed;

        for (int enemyCount = 0; enemyCount < numberOfEnemies; enemyCount++)
        {
            PathFollower newEnemy = Instantiate(enemyPrefab,
                pathPrefab.path.GetPoint(0),
                Quaternion.identity);
            newEnemy.pathCreator = pathPrefab;
            newEnemy.speed = moveSpeed;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        pathsFinished += 1;
        Debug.Log(pathsFinished);
    }
}
