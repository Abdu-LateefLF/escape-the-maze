using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject preLoadCanvas;
    public Slider progressBar;

    public GameObject loadingCamera;
    public GameObject preLoadCamera;

    public void OpenScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        loadingCamera.SetActive(true);

        preLoadCamera.SetActive(false);
        preLoadCanvas.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}
