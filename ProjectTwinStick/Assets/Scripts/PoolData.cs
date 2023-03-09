using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolData<T> where T : UnityEngine.Object
{
    public int startCount;
    public T prefab;
}