using HelperPSR.Pool;
using Unity.VisualScripting;
using UnityEngine;

// spawnEvent -> random vs scripté (SO ?)

namespace Game.Systems.AI
{
    public class Spawner : MonoBehaviour
    {
        // list of SpawnEventTypes ?
        public Pool<BasicAI> Pool;
        [SerializeField] private GameObject objToSpawn;
        [SerializeField, Range(0, 10)] private float firstSpawnDelay = 0f;
        [SerializeField, Range(1, 30)] private float secondsBetweenSpawns = 5f;

        public void Initialize()
        {
            InvokeRepeating(nameof(Spawn), firstSpawnDelay, secondsBetweenSpawns);
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
            obj.Init();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }
}