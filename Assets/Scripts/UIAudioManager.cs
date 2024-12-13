using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource btnClick;
    public AudioSource sliderClick;

    public void ClickButton()
    {
        btnClick.Play();
    }

    public void SliderMove()
    {
        if (Time.timeSinceLevelLoad > 0.6f)
        {
            if (sliderClick.isPlaying == false)
                sliderClick.Play();
        }
    }
}
