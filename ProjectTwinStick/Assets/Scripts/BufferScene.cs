using UnityEngine;
using UnityEngine.SceneManagement;

public class BufferScene : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(LoadScene), 1f); 
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
