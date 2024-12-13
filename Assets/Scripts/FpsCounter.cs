using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    public bool average;

    Text fpsText;

    private void Start()
    {
        fpsText = GetComponent<Text>();

        if (!average)
            ShowFPS();
    }

    // Update is called once per frame
    void Update()
    {
        if (average)
            fpsText.text = "AVG FPS: " + ((int)(Time.frameCount / Time.time)).ToString();
    }

    void ShowFPS()
    {
        StartCoroutine(UpdateFPS());
    }

    IEnumerator UpdateFPS()
    {
        fpsText.text = "FPS: " + ((int)(1f / Time.smoothDeltaTime)).ToString();

        yield return new WaitForSeconds(1.5f);

        ShowFPS();
    }
}
