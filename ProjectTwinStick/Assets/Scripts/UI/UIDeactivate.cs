using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeactivate : MonoBehaviour
{
    [SerializeField] float timer;
    public void OnActivate()
    {
        Invoke("DestroySelf", timer);
    }

    void DestroySelf()
    {
        gameObject.SetActive(false);
    }
}
