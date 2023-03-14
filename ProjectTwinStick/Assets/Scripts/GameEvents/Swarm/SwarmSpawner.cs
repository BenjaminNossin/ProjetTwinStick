using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SwarmSpawner : MonoBehaviour
{
    public Transform assignedBarricade;
    private void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position,2f);

        if (assignedBarricade)
        {
            Gizmos.DrawLine(transform.position, assignedBarricade.position);
        }
    }
}
