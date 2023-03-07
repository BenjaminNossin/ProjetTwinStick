using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShooterItemSO : ItemSO
{
    [FormerlySerializedAs("PlayerBulletPrefab")] public Bullet bulletPrefab;
    public int StartCount;
    public float FrequencyBetweenBullet;
    public float DamageBullet;
    public float MinDispersion;
    public float MaxDispersion;

}
