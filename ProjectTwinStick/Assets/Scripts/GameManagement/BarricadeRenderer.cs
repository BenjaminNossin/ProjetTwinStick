using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeRenderer : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    [SerializeField] private Barricade _barricade;
    [SerializeField] private float activateRendererTime;
    private float rendererTimer;
    private bool isActivatedRenderer;
    private bool isDeactivatedRenderer;

    public void Init()
    {
        rendererTimer = 0;
        isActivatedRenderer = true;
        isDeactivatedRenderer = false;
    }

    public void DeactivateBarricadeRenderer()
    {
        isActivatedRenderer = false;
        isDeactivatedRenderer = true;
    }

    public void ActivateBarricadeRenderer()
    {
        isActivatedRenderer = true;
        isDeactivatedRenderer = false;
    }

    private void Update()
    {
        if (isDeactivatedRenderer)
        {
            rendererTimer -= Time.deltaTime;

            _renderer.material.SetFloat("_Dissolve_0_1", rendererTimer / activateRendererTime);

            if (rendererTimer <= 0)
            {
                rendererTimer = 0;
                isDeactivatedRenderer = false;
            }
        }
        else if (isActivatedRenderer)
        {
            rendererTimer += Time.deltaTime;

            _renderer.material.SetFloat("_Dissolve_0_1", (rendererTimer / activateRendererTime));

            if (rendererTimer > activateRendererTime)
            {
                isActivatedRenderer = false;
            }
        }
    }
}