using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPitch : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    float basePitch;

    [SerializeField] float randomMin, randomMax;
    [SerializeField] bool playOnStart;

    private void Start()
    {
        basePitch = audioSource.pitch;
        Pitcher();
        if (playOnStart)
        {
            audioSource.Play();
            Debug.Log("Play BOOM");
        }
    }

    public void Pitcher()
    {
        audioSource.pitch = basePitch + Random.Range(randomMin, randomMax);
    }
}
