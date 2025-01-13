using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Init()
    {
        LoadScene("menu");
        LoadScene("game");
    }

    public void ReloadGame()
    { 
        StartCoroutine(ReloadAsync());
    }

    IEnumerator ReloadAsync()
    {
        DeloadScene("game");

        yield return new WaitForSeconds(5);

        LoadScene("game");
    }

    void LoadScene(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void DeloadScene(string sceneName)
    {
        //SceneManager.UnloadSceneAsync
        SceneManager.UnloadScene(sceneName);
    }
}
