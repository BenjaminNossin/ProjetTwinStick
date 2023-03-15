using Game.Systems.GlobalFramework;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public void BackToMainMenu()
    {
        GameManager.Instance.Initialize(); 
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
