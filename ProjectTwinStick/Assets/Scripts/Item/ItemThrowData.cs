using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemThrowData : ScriptableObject
{
    [Header("throw")] 
    public float ThrowSpeed = 2f;
    public float ThrowHeight = 2f;
    public float BounceDistance = 2f;
    public AnimationCurve ThrowCurve;
    public AnimationCurve BounceCurve;
    public LayerMask BlockerMask;
    public LayerMask PlayerMask;
}
