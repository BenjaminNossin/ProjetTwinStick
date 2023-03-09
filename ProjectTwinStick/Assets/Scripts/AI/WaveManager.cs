using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;
using Game.Systems.GlobalFramework;


namespace Game.Systems.AI
{
    public class WaveManager : MonoBehaviour
    {
        private readonly List<Spawner> spawners = new();
        [SerializeField] private EnemyPool _enemyPools;
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
                spawners[i].Pool = _enemyPools.enemyPools[0];
            }

            foreach (var item in spawners)
            {
                item.Initialize();
            }
        }

        public void OnGameOver()
        {
            Debug.Log($"Stopping Waves");

            foreach (var item in spawners)
            {
                item.StopSpawnings(); 
            }
        }
    }
}

