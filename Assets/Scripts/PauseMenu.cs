using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public SnakeMesh snakeBody;

    public AdsManager adsManager;

    bool isPauseMenuOpen = false;

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        snakeBody.enabled = false;

        isPauseMenuOpen = true;
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        snakeBody.enabled = true;

        isPauseMenuOpen = false;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        adsManager.PlayAd();
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        adsManager.PlayAd();
        SceneManager.LoadScene("MainGame");
    }
}
