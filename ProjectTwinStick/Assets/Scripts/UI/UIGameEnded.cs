using Game.Systems.GlobalFramework;
using UnityEngine;

public class UIGameEnded : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("game ended. Reloading context");
        GameManager.Instance.HideTimer();

        Invoke(nameof(BackToMainMenu), 5f);
    }

    private void BackToMainMenu()
    {
        GameManager.Instance.ReloadContext(); 
    }

    public void Quit()
    {
        GameManager.Instance.OnGameQuit(); 

    }
}
