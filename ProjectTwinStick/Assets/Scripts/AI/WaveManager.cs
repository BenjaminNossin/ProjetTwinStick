using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using Game.Systems.GlobalFramework;
using UnityEngine.Serialization;


namespace Game.Systems.AI
{
    public class WaveManager : MonoBehaviour
    {
        private readonly List<Spawner> spawners = new();
        [FormerlySerializedAs("_enemyPools")] [SerializeField] private EnemyPoolManager enemyPoolsManager;
        private void Start()
        {
            GameManager.Instance.AddWaveManager(this); 
        }

        public void OnGameStart()
        {
            
            Debug.Log($"Starting Waves");

            for (int i = 0; i < transform.childCount; i++)
            {
                spawners.Add(transform.GetChild(i).GetComponent<Spawner>());
                spawners[i].Pool = enemyPoolsManager.enemyPools[0];
            }

            foreach (var item in spawners)
            {
                return; 
                item.Initialize();
            }
        }

        public void OnGameOver()
        {
            return;

            Debug.Log($"Stopping Waves");

            foreach (var item in spawners)
            {
                item.StopSpawnings(); 
            }
        }
    }
}

