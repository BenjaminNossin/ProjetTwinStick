using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Item : MonoBehaviour, IShootable, IDropable, ITakeable, IThrowable
 {
  private ItemSO _so;
  
  public abstract ItemSO GetSO(); 
  public abstract void Shoot();

 public virtual void Drop()
 {

 }

 public virtual void Take()
 {

 }

 public virtual void Throw()
 {
  
 }
 
 }
