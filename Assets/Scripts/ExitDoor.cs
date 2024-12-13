using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class ExitDoor : MonoBehaviour
{
    public AcheivementsManager achievementsManager;
    public GameHandler gameHandler;
    public CharacterQuipManager quipManager;
    public MiniMap miniMap;
     
    private Animator animator;

    [HideInInspector]
    public int requiredAmountOfSwitches = 4;

    public int numSwitchesOn = 0;

    public GameObject snake;
    public GameObject player;
    public GameObject objectiveText;
    public Image fadeScreen;

    [Range(0f, 1f)]
    public float fadeAlpha = 0;

    Color imageColor;

    void Start()
    {
        imageColor = fadeScreen.color;
        fadeScreen.gameObject.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        imageColor.a = fadeAlpha;
        fadeScreen.color = imageColor;
    }

    public void Interact()
    {
        if (numSwitchesOn >= requiredAmountOfSwitches)
        {
            snake.GetComponent<Snake>().enabled = false;
            snake.GetComponent<SnakeMesh>().enabled = false;
            snake.GetComponent<NavMeshAgent>().enabled = false;

            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Player>().enabled = false;
            player.GetComponent<Tracker>().enabled = false;

            fadeScreen.gameObject.SetActive(true);
            animator.Play("Open Exit Door");

            objectiveText.SetActive(false);

            // Update mode settings
            switch (MainMenu.mode)
            {
                case 0:
                    MainMenu.completedEasyMode = true;
                    PlayerPrefs.SetInt("CompletedEasyMode", 1);
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_easy_peasy);
                    break;
                case 1:
                    MainMenu.completedNormalMode = true;
                    PlayerPrefs.SetInt("CompletedNormalMode", 1);
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_pretty_standard);
                    break;
                case 2:
                    MainMenu.completedHardMode = true;
                    PlayerPrefs.SetInt("CompletedHardMode", 1);
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_now_its_getting_serious);
                    break;
                case 3:
                    MainMenu.completedVeryHardMode = true;
                    PlayerPrefs.SetInt("CompletedVeryHardMode", 1);
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_the_one_who_beat_impossible);
                    break;
                case 4:
                    MainMenu.completedExtremeMode = true;
                    PlayerPrefs.SetInt("CompletedExtremeMode", 1);
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_the_top_5);
                    break;
            }

            gameHandler.BeatGame();

            // Check if all modes have been completed
            if (MainMenu.completedEasyMode && MainMenu.completedNormalMode && MainMenu.completedHardMode && MainMenu.completedVeryHardMode && MainMenu.completedExtremeMode)
            {
                achievementsManager.DoGrantAchievement(GPGSIds.achievement_survivor);
            }
        }
    }

    public void LoadCutScene()
    {
        SceneManager.LoadScene("EscapeScene");
    }
}
