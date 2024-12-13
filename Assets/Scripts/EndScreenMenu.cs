using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenMenu : MonoBehaviour
{
    public AdsManager adsManager;

    public void ReturnToMenu()
    {
        adsManager.PlayAd();
        SceneManager.LoadScene("MainMenu");
    }
}
