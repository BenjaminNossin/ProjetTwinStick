using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.AI
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<Spawner> spawners = new();  

        void Start()
        {
            foreach (var item in spawners)
            {
                item.Initialize(); 
            }
        }
    }
}

