using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventData : ScriptableObject
{
   public abstract Type GetTypeEvent();
   public GameEventCallback[] callbacks;

}
