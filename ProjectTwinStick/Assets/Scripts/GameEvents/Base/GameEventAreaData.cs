using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventAreaData : GameEventData
{
   public int AreaTargetCount;
   public GameplayTag TagEvent ;
   public GameplayTag[] CurrentEventInAreaBlockerTag;
   public GameplayTag[] LastEventInAreaBlockerTag;
   public bool IsRememberEvent = true;
}
