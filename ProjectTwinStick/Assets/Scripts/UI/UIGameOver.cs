using Game.Systems.GlobalFramework;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public void Replay()
    {
        GameManager.Instance.Initialize(); 

    }

    public void Quit()
    {
        GameManager.Instance.OnGameQuit(); 

    }
}
