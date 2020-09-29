using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using System;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Serializable]
    public struct Prefabs
    {
        public PathFollower enemyPrefab;
        public PathCreator pathPrefab;
        public float timeBetweenSpawns;
        public int numberOfEnemies;
        public float moveSpeed;
    }

    public Prefabs[] prefabs;

    public Prefabs[] GetPrefabs()
    {
        return prefabs;
    }
}
