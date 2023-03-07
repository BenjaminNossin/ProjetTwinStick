using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    private bool isActive = false;

    public void ActivateController()
    {
        isActive = true;
    }

	public void DeactivateController(){
		isActive = false;
	}

}
