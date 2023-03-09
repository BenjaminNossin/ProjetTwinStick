using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")] public float Speed;
    public float AccelerationTime = 1f;
    public float DecelerationTime = 0.1f;
    [FormerlySerializedAs("InputThreshold")] [Range(0.001f,1)] public float MovementInputThreshold = 0.1f;
    [Header("Aim")] public float RestrictedAimSpeed = 1f;
    [Range(0.001f,1)] public float AimInputThreshold = 0.1f;
}
