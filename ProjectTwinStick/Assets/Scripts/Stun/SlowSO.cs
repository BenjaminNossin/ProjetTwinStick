using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Slow", menuName = "Gameplay/Slow", order = 1)]
public class SlowSO : ScriptableObject
{
    public bool IndefiniteSlow;
    [FormerlySerializedAs("StunDuration")] public float SlowDuration = 0.5f;
    public float SlowMultiplier = 0.5f;
    public List<GameplayTag> TagsToAdd = new List<GameplayTag>();
}
