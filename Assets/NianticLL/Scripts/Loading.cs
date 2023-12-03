using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour
{
    public Slider progressBar;
    public float duration = 5f;

    void Start()
    {
        // Start the loading process in a coroutine
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        // Replace "YourSceneName" with the name of your scene to load
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("CustomMap");

        // Don't let the scene activate until we allow it
        asyncOperation.allowSceneActivation = false;

        //// Update the progress bar while loading
        //while (!asyncOperation.isDone)
        //{
        //    float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the completion value
        //    progressBar.value = progress;

        //    // Check if the loading is almost complete (0.9 is when it's ready to activate)
        //    if (asyncOperation.progress >= 0.9f)
        //    {
        //        // The scene is ready, let's activate it
        //        asyncOperation.allowSceneActivation = true;
        //    }

        //    yield return null;
        //}


        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Lerp(0, 1, t / duration);
            progressBar.value = progress;
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
