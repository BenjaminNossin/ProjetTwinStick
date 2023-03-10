using UnityEngine;
using Game.Systems.GlobalFramework;

public class WaitRoom : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddWaitRoomObj(gameObject); 
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.UpdatePlayerReadyCount(1);       
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.UpdatePlayerReadyCount(-1);  
    }
}
