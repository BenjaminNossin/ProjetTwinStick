using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
 void Upgrade();

 void Degrade();

 void ResetUpgrade();
}