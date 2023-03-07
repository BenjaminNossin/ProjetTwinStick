using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.AI
{
    public class WaveManager : MonoBehaviour
    {
        private readonly List<Spawner> spawners = new();

        private void Start()
        {
            GameManager.Instance.AddWaveManager(this); 
        }

        public void Initialize()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                spawners.Add(transform.GetChild(i).GetComponent<Spawner>());
            }

            foreach (var item in spawners)
            {
                item.Initialize();
            }
        }
    }
}

