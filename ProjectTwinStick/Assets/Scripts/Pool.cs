using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelperPSR.Pool
{
  public class Pool<T> where T : UnityEngine.Object

  {
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T defaultItem;

    public Pool(T defaultItem, int startElementCount = 0, Action<T> callbackForEachElement = null)
    {
      this.defaultItem = defaultItem;
      for (int i = 0; i < startElementCount; i++)
      {
       var currentElement = UnityEngine.Object.Instantiate(defaultItem);
       AddToPool(currentElement,callbackForEachElement);
      }
      
    }

    public void AddToPool(T toAdd, Action<T> callbackForEachElement = null)
    {
      if (toAdd is GameObject t)
      {
        t.SetActive(false);
      }
      else if (toAdd is MonoBehaviour mono)
      {
        mono.gameObject.SetActive(false);
      }
      callbackForEachElement?.Invoke(toAdd);
      pool.Enqueue(toAdd);
    }

    public T GetFromPool(Action<T> callbackForEachElement = null)
    {
      T lastElement;
      if (pool.Count > 0)
      {
        lastElement = pool.Dequeue();
      }
      else
      {
        lastElement =UnityEngine.Object.Instantiate(defaultItem);
      }
      if (lastElement is GameObject t)
      {
        t.SetActive(true);
      }
      else if (lastElement is MonoBehaviour mono)
      {
        mono.gameObject.SetActive(true);
      }
      callbackForEachElement?.Invoke(lastElement);
      return lastElement;
    }

    public IEnumerator AddToPoolLatter(T item, float lifeTime)
    {
      yield return new WaitForSeconds(lifeTime);
      AddToPool(item);
    }
  }

}
