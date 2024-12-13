using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public AcheivementsManager achievementsManager;
    public AdsManager adsManager;
    public PauseMenu pauseMenu;
    public GameObject startInstructionsCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject loadingImage;
    public GameObject mainCanvas;
    public Button respawnButton;
    public Button restartButton;

    int retriesLeft = 2;

    [Header("Modes")]
    public int retriesOnEasy = -1;
    public int retriesOnNormal = 2;
    public int retriesOnHard = 1;
    public int retriesOnVeryHard = 1;
    public int retriesOnExtreme = 0;


    [Header("Respawn")]
    public GameObject player;
    public Tracker tracker;
    public GameObject playerCamera;
    public GameObject monster;
    public float minDistanceAwayFromMonster = 60f;
    public float minDistanceAwayFromBody = 6f;

    // Stats
    [HideInInspector]
    public int numDeaths = 0;

    // Start is called before the first frame update
    public void Awake()
    {
        // Pause game until they press start
        Time.timeScale = 0;
        startInstructionsCanvas.SetActive(true);
        restartButton.gameObject.SetActive(false);

        // Set max retries;
        switch (MainMenu.mode)
        {
            case 0:
                retriesLeft = retriesOnEasy;
                break;
            case 1:
                retriesLeft = retriesOnNormal;
                break;
            case 2:
                retriesLeft = retriesOnHard;
                break;
            case 3:
                retriesLeft = retriesOnVeryHard;
                break;
            case 4:
                retriesLeft = retriesOnExtreme;
                break;
        }

        // Set Player Spawn
        Transform spawn = FindPlayerSpawnLocation();
        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;

        adsManager.HideBanner();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startInstructionsCanvas.SetActive(false);

        // Give player the first timer achievement
        achievementsManager.DoGrantAchievement(GPGSIds.achievement_welcome_to_the_maze);
    }

    private void Start()
    {
        StartCoroutine(player.GetComponent<Player>().WakeUp());

        // Disable button if the player has no more tries (Extreme mode, etc)
        if (retriesLeft == 0)
        {
            respawnButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void FailedToLoadAd()
    {
        loadingImage.SetActive(false);
    }

    private void Update()
    {
    }

    bool CanRespawn()
    {
        bool can = false;

        if (retriesLeft > 0)
        {
            can = true;
        }
       
        // If on easy mode
        else if (retriesLeft == -1)
        {
            can = true;
        }

        return can;
    }

    Transform FindPlayerSpawnLocation()
    {
        GameObject[] possibleLocations = GameObject.FindGameObjectsWithTag("Player Spawn Location");

        int index = 0;
        bool isValidLocation = false;

        while (isValidLocation == false)
        {
            isValidLocation = false;

            index = Random.Range(0, possibleLocations.Length);

            if (Vector3.Distance(possibleLocations[index].transform.position, monster.transform.position) > minDistanceAwayFromMonster)
            {
                bool farFromBody = true;

                SnakeMesh snakeBody = monster.GetComponent<SnakeMesh>();
                for (int i = 0; i < snakeBody.points.Count; i++)
                {
                    if (Vector3.Distance(possibleLocations[index].transform.position, snakeBody.points[i].transform.position) < minDistanceAwayFromBody)
                        farFromBody = false;
                }

                if (farFromBody)
                {
                    isValidLocation = true;
                    break;
                }
            }
        }

        return possibleLocations[index].transform;
    }

    public void TryToRespawn()
    {
        if (CanRespawn())
        {
            loadingImage.SetActive(true);

            // Play Reward Ad
            adsManager.PlayRewardAd();
        }
    }

    public void RespawnPlayer()
    {
        if (retriesLeft != -1)
        {
            retriesLeft--;
        }

        numDeaths++;    // Update Stats

        loadingImage.SetActive(false);
        gameOverCanvas.SetActive(false);
        mainCanvas.SetActive(true);

        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovement>().sanity = 100f;
        player.GetComponent<PlayerMovement>().flashlight.SetActive(true);
        player.GetComponent<PlayerMovement>().cam.fieldOfView = player.GetComponent<PlayerMovement>().initCamFov;
        player.GetComponent<Player>().alive = true;
        playerCamera.SetActive(true);

        player.GetComponent<Tracker>().enabled = true;
        player.GetComponent<Tracker>().EnableCell();
        player.GetComponent<Tracker>().batteryLevel = 50f;

        //Stats
        player.GetComponent<PlayerMovement>().numRatsStartled = 0;  

        // Relocate Player
        Transform spawn = FindPlayerSpawnLocation();
        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;

        Snake snake = monster.GetComponent<Snake>();
        snake.state = "idle";
        snake.headAnimator.Play("Armature|Patrol");
        snake.deathCam.gameObject.GetComponent<Animator>().Play("Idle");
        snake.deathCam.gameObject.SetActive(false);
        snake.active = false;
        snake.highAlert = false;

        SnakeMesh snakeBody = monster.GetComponent<SnakeMesh>();
        snakeBody.enabled = true;

        for (int i = 0; i < snakeBody.points.Count; i++)
        {
            if (snakeBody.points[i].GetComponent<AudioSource>() != null)
                snakeBody.points[i].GetComponent<AudioSource>().enabled = false;
        }

        StartCoroutine(player.GetComponent<Player>().WakeUp());

        // Disable button if the player has no more tries
        if (retriesLeft == 0)
        {
            respawnButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void BeatGame()
    {
        print("Beat Game");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        Player playerClass = player.GetComponent<Player>();

        Snake snake = monster.GetComponent<Snake>();

        /*
         * Player Related
         */
        // No pills consumed
        if (playerMovement.numPillsConsumed == 0)
        {
            switch (MainMenu.mode)
            {
                case 0:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_lemon_squeezy);
                    break;
                case 1:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_the_normal_way_to_leave);
                    break;
                case 2:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_scary_is_it);
                    break;
                case 3:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_unnecessary_medication);
                    break;
            }
        }
        if (playerMovement.numPillsConsumed <= 1)
        {
            if (MainMenu.mode == 4)
            {
                achievementsManager.DoGrantAchievement(GPGSIds.achievement_the_top_1);
            }
        }

        if (numDeaths == 0)
        {
            switch(MainMenu.mode) 
            {
                case 0:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_lemon_squeezy);
                    break;
                case 1:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_a_walk_in_the_park);
                    break;
                case 2:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_still_not_hard_enough);
                    break;
                case 3:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_ahead_of_the_curve);
                    break;
            }
        }
        /*
         * Snake Related
         */
        if (snake.numTimesSpottedPlayer == 0)
        {
            switch (MainMenu.mode)
            {
                case 0:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_novice_ninja);
                    break;
                case 1:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_amateur_ninja);
                    break;
                case 2:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_intermediate_level_ninja);
                    break;
                case 3:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_advanced_level_ninja);
                    break;
                case 4:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_master_ninja);
                    break;
            }
        }

        // if in very hard mode and no extra lifes used
        if (MainMenu.mode == 3 && retriesLeft == retriesOnVeryHard)
        {
            MainMenu.beatVeryHardModeNoRespawns = true;
            PlayerPrefs.SetInt("BeatVeryHardModeNoRespawns", 1);
        }

    }

    public void CheckRatAchievements()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        print("Scared Rat");

        if (playerMovement.numRatsStartled >= 5)
        {       
            switch (MainMenu.mode)
            {
                case 0:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_thats_not_very_nice);
                    break;
                case 1:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_what_an_abnormal_way_of_greeting_someone);
                    break;
                case 2:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_this_must_be_on_purpose);
                    break;
                case 3:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_terratfic);
                    break;
                case 4:
                    achievementsManager.DoGrantAchievement(GPGSIds.achievement_deep_rooted_hate);
                    break;
            }
        }
    }

    public void CheckBatteryUsedCount()
    {
        print("Battery Found");

        if (tracker.numBatteriesUsed >= 10)
        {
            achievementsManager.DoGrantAchievement(GPGSIds.achievement_not_very_efficient);
        }
    }
}
