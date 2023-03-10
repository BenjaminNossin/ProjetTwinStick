using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Arena", LoadSceneMode.Single); 

    }

    public void QuitGame()
    {
        Debug.Log("Application Quit"); 
        Application.Quit();
    }
}
