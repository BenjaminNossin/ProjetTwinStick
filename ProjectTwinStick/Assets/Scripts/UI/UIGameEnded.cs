using Game.Systems.GlobalFramework;
using UnityEngine;

public class UIGameEnded : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("game ended. Reloading context"); 
        Invoke(nameof(BackToMainMenu), 5f);
    }

    private void BackToMainMenu()
    {
        GameManager.Instance.InitializeContext(); 
    }

    public void Quit()
    {
        GameManager.Instance.OnGameQuit(); 

    }
}
