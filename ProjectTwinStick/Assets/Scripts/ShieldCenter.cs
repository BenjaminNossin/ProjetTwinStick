
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCenter : MonoBehaviour
{
    public float Radius = 10f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
