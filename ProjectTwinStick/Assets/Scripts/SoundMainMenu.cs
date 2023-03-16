using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMainMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    [SerializeField] Slider soundSlider;

    public void ChangeValue()
    {
        mainMixer.SetFloat("Master", soundSlider.value);
    }
}
