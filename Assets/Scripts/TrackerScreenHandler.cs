using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerScreenHandler : MonoBehaviour
{
    public Tracker tracker;
    public AudioSource buttonClick;

    public Button leftArrow;
    public Button rightArrow;

    public void SwitchToScreen1()
    {
        if (tracker.enabled)
        {
            tracker.ShowScreen1();
            ButtonPress();
            print("Called");

            leftArrow.interactable = false;
            rightArrow.interactable = true;
        }
    }

    public void SwitchToScreen2()
    {
        if (tracker.enabled)
        {
            tracker.ShowScreen2();
            ButtonPress();
            print("Called");

            leftArrow.interactable = true;
            rightArrow.interactable = false;
        }
    }

    public void ButtonPress()
    {
        buttonClick.Play(); 
    }
}
