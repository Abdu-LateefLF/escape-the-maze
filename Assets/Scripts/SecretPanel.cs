using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecretPanel : MonoBehaviour
{
    public AcheivementsManager achievementsManager;
    public TMP_InputField codeField;
    public GameObject panel;
    public int numPresses = 20;

    private int presses = 0;

    public void RegisterPress()
    {
        presses++;
        if (presses >= numPresses)
        {
            panel.SetActive(true);

            //achievementsManager.DoGrantAchievement(GPGSIds.achievement_cheating_really);
        }
    }

    public void EnterCode()
    {
        if (codeField.text == "95F150DRAKETHEGOAT2!")
        {
            // Make very hard mode complete
            PlayerPrefs.SetInt("CompletedVeryHardMode", 1);

            SceneManager.LoadScene(0);
        }
    }
}
