using HelperPSR.Pool;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Serialization;

// spawnEvent -> random vs scripté (SO ?)

namespace Game.Systems.AI
{
    public class Spawner : MonoBehaviour
    {
        // list of SpawnEventTypes ?
        public Pool<BasicAI> Pool;
        [SerializeField] Transform assignedBarricade; 
        [SerializeField] private GameObject objToSpawn;
        [SerializeField, Range(0, 10)] private float firstSpawnDelay = 0f;
        [SerializeField] private SpawnerParams spawnerParams;
        [SerializeField] private EnemyStats defaultStats;

        private EnemyStats _currentStats;
        private SpawnerParams _currentSpawnerParams;

        public void Initialize()
        {
            Invoke(nameof(Spawn), firstSpawnDelay);
            _currentStats = defaultStats;
            _currentSpawnerParams = spawnerParams;

        }
        
        public void ChangeStats(EnemyStats newStats)
        {
            _currentStats = newStats;
        }
        
        public void ChangeParams(SpawnerParams newParams)
        {
            _currentSpawnerParams = newParams;
        }

        public void StopSpawnings()
        {
            CancelInvoke(nameof(Spawn));
        }

        private void Spawn()
        {
            var obj = Pool.GetFromPool();
            obj.transform.position = transform.position;
            obj._pool = Pool;
            obj.Init(assignedBarricade.position, _currentStats);
            float waitTime = UnityEngine.Random.Range(_currentSpawnerParams.spawnDelayMin, _currentSpawnerParams.spawnDelayMax);
            //Debug.Log(waitTime);
            Invoke(nameof(Spawn), waitTime);
        }

        /*IEnumerator Spawning()
        {

        }*/


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f);

            if (assignedBarricade)
            {
                Gizmos.DrawLine(transform.position, assignedBarricade.position);
            }
        }
    }
}