using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetBoolParameterTrue(string name)
    {animator.SetBool(name, true);
    }

    public void SetBoolParameterFalse(string name)
    {
        animator.SetBool(name, false);
    }

    public void SetFloatParameter(string name, float value)
    {
        animator.SetFloat(name, value);
    }
}
