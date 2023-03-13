using System;
using System.Collections;
using System.Collections.Generic;
using HelperPSR.Pool;
using UnityEngine;

public class PoolFeedbackManager : MonoBehaviour
{
 [SerializeField]
 private PoolData<GameObject>[] poolDataFeedbacks;

 private Dictionary<GameObject,Pool<GameObject>> pools = new Dictionary<GameObject, Pool<GameObject>>();

 public static PoolFeedbackManager instance; 
 private void Awake()
 {
     instance = this;
     
     for (int i = 0; i < poolDataFeedbacks.Length; i++)
     {
         pools.Add(poolDataFeedbacks[i].prefab,new Pool<GameObject>(poolDataFeedbacks[i].prefab, poolDataFeedbacks[i].startCount));
     }
 }

 public GameObject GetFromPool(GameObject prefab)
 {
     return pools[prefab].GetFromPool();
 }

 public GameObject GetFromPool(GameObject prefab, float time)
 {
     var obj = pools[prefab].GetFromPool();
     pools[prefab].AddToPoolLatter(obj,time);
     return obj;
 }

 public void AddToPool(GameObject prefab, GameObject obj)
 {
     pools[prefab].AddToPool(obj);
 }
 
 
}
