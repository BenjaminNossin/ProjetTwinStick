using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(1).name, LoadSceneMode.Single); 

    }

    public void QuitGame()
    {
        Debug.Log("Application Quit"); 
        Application.Quit();
    }
}
