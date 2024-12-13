using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using GooglePlayGames.BasicApi;
using UnityEngine.Networking;
using TMPro;

public struct Data
{
    public string version;
}

public class MainMenu : MonoBehaviour
{
    public static int mode;
    public static bool completedEasyMode = false;
    public static bool completedNormalMode = false;
    public static bool completedHardMode = false;
    public static bool completedVeryHardMode = false;
    public static bool completedExtremeMode = false;
    public static bool beatVeryHardModeNoRespawns = false;
    public static int playCounter = 0;

    public AdsManager adsManager;
    public LoadScene loadScene;

    [Header("Mode Buttons")]
    public ModeSelectUI easyMode;
    public ModeSelectUI normalMode;
    public ModeSelectUI hardMode;
    public ModeSelectUI veryHardMode;
    public ModeSelectUI extremeMode;

    [Header("Mode Texts")]
    public GameObject easyText;
    public GameObject normalText;
    public GameObject hardText;
    public GameObject veryHardText;
    public GameObject extremeText;

    public Text tipText;

    // Reading data from json 
    private string jsonURL = "https://drive.google.com/uc?export=download&id=1WIgeWuvmbltXx6PKRWkGYoHe-Tt9j1Ky";

    private void Awake()
    {
        // Load Banner
        if (Advertisement.Banner.isLoaded == false)
        {
            LoadBanner();
        }
        else
        {
            adsManager.ShowBanner();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Data from json
        StartCoroutine(GetData(jsonURL));

        mode = 0;
        Time.timeScale = 1;

        UpdateCompletedModes();
        CheckIfBeatenModes();

        AdjustDescriptionText();
    }

    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.disposeDownloadHandlerOnDispose = true;
        request.timeout = 60;

        yield return request.SendWebRequest();

        if (request.isDone) {
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);

            if (!string.IsNullOrEmpty(data.version))
            {
                if (!Application.version.Equals(data.version))
                {
                    // Update available
                    tipText.text = "There is a new update available on play store!";
                    tipText.color = Color.red;
                }
                else
                {
                    print("Version up to date!");
                }
            }
            else
            {
                print("Recieved version string empty!");
            }
        }
    }

    void LoadBanner()
    {
        print("Not yet initialized");
        StartCoroutine(TryLoadBanner());
    }

    IEnumerator TryLoadBanner()
    {
        yield return new WaitForSeconds(1f);

        // Load Banner
        if (Advertisement.isInitialized)
        {
            adsManager.LoadBanner();
        }
        else
        {
            LoadBanner();
        }
    }

    void UpdateCompletedModes()
    {
        // Get Saved Values if possible
        if (PlayerPrefs.HasKey("CompletedEasyMode"))
            completedEasyMode = PlayerPrefs.GetInt("CompletedEasyMode") > 0;
        if (PlayerPrefs.HasKey("CompletedNormalMode"))
            completedNormalMode = PlayerPrefs.GetInt("CompletedNormalMode") > 0;
        if (PlayerPrefs.HasKey("CompletedHardMode"))
            completedHardMode = PlayerPrefs.GetInt("CompletedHardMode") > 0;
        if (PlayerPrefs.HasKey("CompletedVeryHardMode"))
            completedVeryHardMode = PlayerPrefs.GetInt("CompletedVeryHardMode") > 0;
        if (PlayerPrefs.HasKey("BeatVeryHardModeNoRespawns"))
            beatVeryHardModeNoRespawns = PlayerPrefs.GetInt("BeatVeryHardModeNoRespawns") > 0;
        if (PlayerPrefs.HasKey("CompletedExtremeMode"))
            completedExtremeMode = PlayerPrefs.GetInt("CompletedExtremeMode") > 0;

        if (completedEasyMode)
            easyMode.SetComplete();

        if (completedNormalMode)
            normalMode.SetComplete();

        if (completedHardMode)
            hardMode.SetComplete();

        if (completedVeryHardMode)
        {
            /*
            if (!beatVeryHardModeNoRespawns)
                veryHardText.color = Color.green;
            else
                veryHardText.color = Color.magenta;
            */

            veryHardMode.SetComplete();
        }

        if (completedExtremeMode)
            extremeMode.SetComplete();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void CheckIfBeatenModes()
    {
        // Check if the player has beat easy mode
        if (!completedNormalMode && !completedHardMode && !completedVeryHardMode)
        {
            hardMode.LockMode();
            veryHardMode.LockMode();
        }

        if (!completedVeryHardMode)
        {
            extremeMode.LockMode();
        }
    }

    public void Play()
    {
        // Hide Banner
        adsManager.HideBanner();

        // Update play counter
        playCounter++;
        if (playCounter >= 3)
        {
            playCounter = 0;
            adsManager.PlayAd();
        }

        loadScene.OpenScene("MainGame");
    }

    public void OnModeSelected()
    {
        AdjustDescriptionText();
    }

    void AdjustDescriptionText()
    {
        easyText.SetActive(false);
        normalText.SetActive(false);    
        hardText.SetActive(false);
        veryHardText.SetActive(false);  
        extremeText.SetActive(false);  

        switch (mode)
        {
            case 0:
                easyText.SetActive(true);
                break;
            case 1:
                normalText.SetActive(true); 
                break;
            case 2:
                hardText.SetActive(true);   
                break;
            case 3:
                veryHardText.SetActive(true);   
                break;
            case 4:
                extremeText.SetActive(true);
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
