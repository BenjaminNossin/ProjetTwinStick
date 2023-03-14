using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
  bool TryShoot(Vector3 startPosition, Vector2 direction);
}
