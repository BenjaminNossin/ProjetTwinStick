using Game.Systems.GlobalFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single); 
    }

    public void Quit()
    {
        GameManager.Instance.OnGameQuit(); 

    }
}
