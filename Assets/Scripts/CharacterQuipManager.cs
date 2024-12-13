using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Subtitle
{
    public string text;
    public float screenTime;
};

public class CharacterQuipManager : MonoBehaviour
{
    public Text subtitleText;

    private List<Subtitle> subtitleQueue = new List<Subtitle>();
    private float timeOnScreen;

    void Start()
    {
        subtitleText.gameObject.SetActive(false);
        timeOnScreen = 0;
    }

    void Update()
    {
        if (subtitleQueue.Count > 0)
        {
            DisplaySubtitles();
        }
        else
        {
            subtitleText.gameObject.SetActive(false);
            timeOnScreen = 0;
        }
    }

    public void ClearSubtitleQueue()
    {
        subtitleQueue.Clear();
        timeOnScreen = 0;
    }

    public void AddSubtitleToQueue(string text, float time)
    {
        Subtitle newSubtitle = new Subtitle();
        newSubtitle.text = text;
        newSubtitle.screenTime = time;

        subtitleQueue.Add(newSubtitle);
    }

    void DisplaySubtitles()
    {
        Subtitle subtitle = subtitleQueue[0];
            
        timeOnScreen += Time.deltaTime;

        if (timeOnScreen < subtitle.screenTime)
        {
            subtitleText.gameObject.SetActive(true);
            subtitleText.text = subtitle.text;
        }

        else
        {
            subtitleText.gameObject.SetActive(false);
            subtitleText.text = "";

            subtitleQueue.Remove(subtitle);

            timeOnScreen = 0;
        }
    }
}
