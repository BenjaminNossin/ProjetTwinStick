using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeable
{
    public bool CanTake();
    public void Take(GameObject holder);
}
