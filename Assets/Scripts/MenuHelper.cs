using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHelper : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabs;

    public void ResetAll()
    {
        ResetTabButtons();
        ResetTabs();
    }

    public void ResetTabButtons()
    {
        for (int i = 0; i < tabButtons.Length;  i++)
        {
            tabButtons[i].interactable = true;
        }
    }

    public void ResetTabs()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }
    }

}
