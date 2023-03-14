using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_UpdateMaterial : MonoBehaviour
{
    public SkinnedMeshRenderer _skinRenderer;

    void Update(){

        
           _skinRenderer.material.SetFloat("_HitBlanc", Mathf.Sin(Time.time));

    }

}
