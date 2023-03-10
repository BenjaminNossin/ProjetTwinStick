using HelperPSR.Pool;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;

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
        [SerializeField, Range(1, 30)] private float randomMin = 5f;
        [SerializeField] private float randomMax = 5f;

        public void Initialize()
        {
            Invoke(nameof(Spawn), UnityEngine.Random.Range(randomMin,randomMax));
            
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
            obj.Init(assignedBarricade.position);
            float waitTime = UnityEngine.Random.Range(randomMin, randomMax);
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