using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShooterUpgrade : ItemUpgrade
{
    public float FrequencyBetweenBullet;
    public float DamageBullet;
    [SerializeField] public float MinDispersionDegres;
    [HideInInspector] public float MinDispersionRadian;
    [HideInInspector] public float MaxDispersionRadian;
    [SerializeField] public float MaxDispersionDegres;
    public float Speed;
    public SlowSO slowType;
}
