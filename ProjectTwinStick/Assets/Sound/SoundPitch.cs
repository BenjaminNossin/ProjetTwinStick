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
    }

    public void PlayPliz()
    {
        Debug.Log("BOOM");
        Pitcher();
        audioSource.Play();
    }

    public void Pitcher()
    {
        audioSource.pitch = basePitch + Random.Range(randomMin, randomMax);
    }
}
