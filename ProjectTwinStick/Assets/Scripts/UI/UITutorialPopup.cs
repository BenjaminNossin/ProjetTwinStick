using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UITutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject panel; 
    [SerializeField] private TextMeshProUGUI UItext;
    private float Timer = 0f;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                panel.SetActive(false);
            }
        }
    }
    
    public void SetPopup(string text, float time)
    {
        Debug.Log("setting popup : " + text + " for " + time + " seconds");
        Timer = time;
        panel.SetActive(true);
        UItext.text = text;
    }
}
