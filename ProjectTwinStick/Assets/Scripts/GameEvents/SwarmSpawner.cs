using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SwarmSpawner : MonoBehaviour
{
    [HideInInspector]
    public bool isAlreadySpawn;
    private void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position,2f);
    }
}
