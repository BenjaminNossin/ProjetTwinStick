using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventAreaData : GameEventData
{
   [Range(1, 4)] public int AreaTargetCount = 1; 
   public GameplayTag TagEvent ;
   public GameplayTag[] CurrentEventInAreaBlockerTag;
   public GameplayTag[] LastEventInAreaBlockerTag;
   [Tooltip("Should this event be remembered by the next event ?")] public bool IsRememberEvent {get; set;}
}
