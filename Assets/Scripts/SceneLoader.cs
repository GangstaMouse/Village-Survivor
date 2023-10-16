using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static AsyncOperation m_SceneUnloadingOperation;
    private static AsyncOperation m_SceneLoadingOperation;

    public static void ReloadScene(int sceneIndex) // should not working!!!
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        UnloadScene(currentSceneIndex);
        LoadScene(currentSceneIndex);
    }

    public static async void LoadScene(int sceneIndex)
    {
        m_SceneLoadingOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (m_SceneLoadingOperation.isDone == false)
        {
            Debug.Log($"Scene loading progress: {m_SceneLoadingOperation.progress}");
            await Task.Yield();
        }
        m_SceneLoadingOperation = null;
    }

    public static async void UnloadScene(int sceneIndex)
    {
        m_SceneUnloadingOperation = SceneManager.UnloadSceneAsync(sceneIndex);

        while (m_SceneUnloadingOperation.isDone == false)
        {
            await Task.Yield();
        }
        m_SceneUnloadingOperation = null;
    }
}
