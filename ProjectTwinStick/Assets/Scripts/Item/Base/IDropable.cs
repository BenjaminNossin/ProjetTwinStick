using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropable
{
 public bool CanDrop();
 public void Drop();
}
