using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectUI : MonoBehaviour
{
    public ModeSelect modeSelect;
    public int modeIndex = 0;
    public TextMeshProUGUI modeText;
    public GameObject compCheckmark;
    public GameObject lockIcon;

    public Color unselectedColor;
    public Color selectedColor;
    public Color beatColor;

    bool beaten = false;
    bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetComplete()
    {
        modeText.SetText("<s>" + modeText.text + "</s>");
        compCheckmark.gameObject.SetActive(true);

        if (!selected)
            GetComponent<Image>().color = beatColor;

        beaten = true;
    }

    public void LockMode()
    {
        lockIcon.SetActive(true);
        this.GetComponent<Button>().enabled = false;
    }

    public void SetMode()
    {
        modeSelect.ResetAllButtons();

        MainMenu.mode = modeIndex;
        GetComponent<Image>().color = selectedColor;

        selected = true;

        GetComponent<RectTransform>().localScale = new Vector3(1.05f, 1.05f, 1.05f);
    }

    public void UnSetMode()
    {
        if (beaten)
            GetComponent<Image>().color = beatColor;
        else
            GetComponent<Image>().color = unselectedColor;

        selected = false;

        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }
}
