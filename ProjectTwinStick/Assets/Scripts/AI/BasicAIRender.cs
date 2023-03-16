using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIRender : MonoBehaviour
{
   [SerializeField] private BasicAI _basicAI;
   [SerializeField] private SlowManager _slowManager;
   [SerializeField] private float minSpeedMovementAnimation;
   [SerializeField] private Animator _animator;
   [SerializeField] private Transform renderer;
   [SerializeField] private ParticleSystem _explosionFXPrefab;
   [SerializeField] private Renderer[] allRenderers;
   
   private bool isDisolve;
   private bool isHit;
   private float _hitTimer;
   [SerializeField] private float dissolveTime;
   private float dissolveTimer;
    [SerializeField] private float _hitTime;
   public void Init()
   {
      _slowManager.OnSlowMultiplierChanged += UpdateSpeedMovementAnimation;
      _basicAI.OnHit += LaunchHitAnimation;
      _basicAI.OnHit += OnHitMaterial;
      _basicAI.OnDieByPlayer += LaunchDieByPlayerAnimation;
      _basicAI.OnDieByPlayer += OnHitMaterial;
      _basicAI.OnSetMoveDirection += RotateRenderer;
      _basicAI.OnDieImmedialty += InstantiateExplosionFX;
   }

   private void OnHitMaterial()
   {
      isHit = true;
      _hitTimer = 0;
      for (int i = 0; i < allRenderers.Length; i++)
      {
         allRenderers[i].material.SetFloat("_HitBlanc", 1);
      }
   }
   private void OnEnable()
   {
      isDisolve = false;
      dissolveTimer = 0;
      for (int i = 0; i < allRenderers.Length; i++)
      {
         allRenderers[i].material.SetFloat("_Dissolve", 1);
      }
   }

   private void StartDisolve()
   {
      isDisolve = true;
      dissolveTimer = 0;
   }

   private void Update()
   {
      if (isDisolve)
      {
         dissolveTimer += Time.deltaTime;
         for (int i = 0; i < allRenderers.Length; i++)
         {
            allRenderers[i].material.SetFloat("_Dissolve", 1-(dissolveTimer/dissolveTime));
         }
         if (dissolveTime < dissolveTimer)
         {
            isDisolve = false;
            dissolveTimer = 0;
         }
      }

      if (isHit)
      {
         _hitTimer += Time.deltaTime;
         for (int i = 0; i < allRenderers.Length; i++)
         {
            allRenderers[i].material.SetFloat("_HitBlanc",1-(_hitTimer/_hitTime));
         }
         if (_hitTime < _hitTimer)
         {
            isHit = false;
            _hitTimer = 0; 
         }
      }
   }

   public void InstantiateExplosionFX()
   {
     var fx = PoolFeedbackManager.instance.GetFromPool(_explosionFXPrefab.gameObject, _explosionFXPrefab.main.duration);
        fx.GetComponent<AudioSource>().Play();
        fx.transform.position = _basicAI.transform.position;
   }
   public void UpdateSpeedMovementAnimation(float value) =>
      _animator.SetFloat("speed", math.remap(0, 1, minSpeedMovementAnimation, 1, value));

   public void LaunchHitAnimation()
   {
      _animator.Play("Hit");
   }
   public void LaunchDieByPlayerAnimation()
   {
      _animator.Play("Die");
   }

   void RotateRenderer(Vector3 dir)
   {
      renderer.forward = dir;
   }
}
