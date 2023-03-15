using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPitch : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    float basePitch;

    [SerializeField] float randomMin, randomMax;

    private void Start()
    {
        basePitch = audioSource.pitch;
    }

    public void Pitcher()
    {
        audioSource.pitch = basePitch + Random.Range(randomMin, randomMax);
    }
}
