using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace PathCreation.Examples {

    public class PathSpawner : MonoBehaviour {

        public PathCreator pathPrefab;
        public PathFollower followerPrefab;
        public int numberOfEnemies = default;
        public Transform[] spawnPoints;

        public PathCreator path;

        private void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        public IEnumerator SpawnEnemies()
        {
            foreach (Transform t in spawnPoints)
            {
                path = Instantiate(pathPrefab, t.position, t.rotation);
                path.transform.parent = t;
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    var followers = Instantiate(followerPrefab, t.position, t.rotation);
                    followers.pathCreator = path;
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

}