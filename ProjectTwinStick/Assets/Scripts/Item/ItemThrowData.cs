using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemThrowData : ScriptableObject
{
    [Header("Grounded")] 
    public float GroundedHeight = 1f;

    [Header("Throw")] 
    public float ThrowChargeTime = 1f;
    public float MinThrowStrength = 1f;
    public float MaxThrowStrength = 10f;
    public float ThrowSpeed = 2f;
    public float ThrowHeight = 2f;
    public AnimationCurve ThrowCurve;
    [Header("Bounce")]
    public float BounceSpeed = 1f;
    public float BounceDistance = 2f;
    public float BounceHeight = 2f;
    public AnimationCurve BounceCurve;
    public LayerMask BlockerMask;
    public LayerMask PlayerMask;
}
