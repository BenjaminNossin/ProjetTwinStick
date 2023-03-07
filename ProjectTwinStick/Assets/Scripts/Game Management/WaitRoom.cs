using UnityEngine;

public class WaitRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.UpdatePlayerReadyCount(1);       
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.UpdatePlayerReadyCount(-1);  
    }
}
