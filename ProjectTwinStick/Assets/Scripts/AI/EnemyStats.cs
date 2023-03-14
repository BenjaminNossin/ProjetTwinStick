using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
    public float maxHP = 10f;  
    public float unitsPerSeconds = 10;
    public float damage = 20f;
}
