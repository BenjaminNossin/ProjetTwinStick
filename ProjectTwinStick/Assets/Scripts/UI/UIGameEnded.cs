using Game.Systems.GlobalFramework;
using UnityEngine;

public class UIGameEnded : MonoBehaviour
{
    public void BackToMainMenu()
    {
        GameManager.Instance.ReloadContext(); 
    }

    public void Replay()
    {
        GameManager.Instance.SetNewState(MainMenuSelections.MainGame);
    }

    public void Quit()
    {
        GameManager.Instance.OnGameQuit(); 

    }
}
