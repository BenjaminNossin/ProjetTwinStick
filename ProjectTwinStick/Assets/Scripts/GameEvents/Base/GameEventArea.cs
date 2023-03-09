using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventArea : GameEvent
{
    protected Area[] targetAreas;

    public void SetAreas(Area[] areas)
    {
        targetAreas = areas;
    }
    
    public Area[] GetArea()
    {
        return targetAreas;
    }
}
