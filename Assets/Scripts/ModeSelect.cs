using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    public ModeSelectUI[] modeButtons;
    // Start is called before the first frame update
    void Start()
    {
        ResetAllButtons();
        modeButtons[0].SetMode();
    }

    public void ResetAllButtons()
    {
        for (int i = 0;  i < modeButtons.Length; i++)
        {
            modeButtons[i].UnSetMode();
        }
    }
}
