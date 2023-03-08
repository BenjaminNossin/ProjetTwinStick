using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Benji_CoreScene", LoadSceneMode.Single); 

    }

    public void QuitGame()
    {
        Debug.Log("Application Quit"); 
        Application.Quit();
    }
}
