using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BasicAIRender : MonoBehaviour
{
   [SerializeField] private BasicAI _basicAI;
   [SerializeField] private SlowManager _slowManager;
   [SerializeField] private float minSpeedMovementAnimation;
   [SerializeField] private Animator _animator;
   [SerializeField] private Transform renderer;
   public void Init()
   {
      _slowManager.OnSlowMultiplierChanged += UpdateSpeedMovementAnimation;
      _basicAI.OnHit += LaunchHitAnimation;
      _basicAI.OnDieByPlayer += LaunchDieByPlayerAnimation;
      _basicAI.OnSetMoveDirection += RotateRenderer;
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
