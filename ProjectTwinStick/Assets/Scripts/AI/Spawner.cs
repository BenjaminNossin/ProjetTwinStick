using UnityEngine;

namespace Game.Systems.AI
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject objToSpawn;
        [SerializeField, Range(0, 10)] private float firstSpawnDelay = 0f;
        [SerializeField, Range(1, 30)] private float secondsBetweenSpawns = 5f; 

        public void Initialize()
        {
            InvokeRepeating(nameof(Spawn), firstSpawnDelay, secondsBetweenSpawns); 
        }

        private void Spawn()
        {
            Instantiate(objToSpawn, transform.position, Quaternion.identity); 
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f); 
        }
    }
}
