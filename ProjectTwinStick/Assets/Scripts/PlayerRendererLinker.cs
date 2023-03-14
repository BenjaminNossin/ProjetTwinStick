using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRendererLinker : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;
    
    public void Link(PlayerRenderer playerRenderer)
    {
        playerRenderer.animator = animator;
        playerRenderer.handTransform = hand;
        playerRenderer.Init();
    }
}
