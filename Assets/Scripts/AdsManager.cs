using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,IUnityAdsShowListener
{
    public bool isTestingMode = true;
    public static bool isPlayingAd = false;
    public GameHandler gameHandler;

#if UNITY_IOS
    string gameId = "5369240";
    string interstitialVideoID = "Interstitial_iOS";
    string rewardVideoID = "Rewarded_iOS";
    string bannerID = "Banner_iOS";
#else
    string gameId = "5369241";
    string interstitialVideoID = "Interstitial_Android";
    string rewardVideoID = "Rewarded_Android";
    string bannerID = "Banner_Android";
#endif

    void Start()
    {
        InitializeAds();
    }
    // Start is called before the first frame update
    void InitializeAds()
    {
        Advertisement.Initialize(gameId, isTestingMode, this);
    }
    public void OnInitializationComplete()
    {
        print("Initialization Completed");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print("Initialization Failed");
    }

    private void OnApplicationQuit()
    {
    }

    private void OnDestroy()
    {
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerLoadError
        };

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);
        Advertisement.Banner.Load(bannerID, options);   
    }

    void OnBannerLoaded()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            ShowBanner();
    }

    void OnBannerLoadError (string error)
    {
        print("failed to load");
    }

    public void ShowBanner()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = OnBannerShown,
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden
        };

        Advertisement.Banner.Show(bannerID, options);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    void OnBannerShown() { }
    void OnBannerClicked() { }
    void OnBannerHidden() { }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void PlayAd()
    {
        Advertisement.Load(interstitialVideoID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (gameHandler != null)
            gameHandler.FailedToLoadAd();
    }

    public void PlayRewardAd()
    {
        Advertisement.Load(rewardVideoID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (gameHandler != null)
            gameHandler.FailedToLoadAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        isPlayingAd = true;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        isPlayingAd = false;
        print("Show complete");

        // Reward video Completed!
        if (placementId.Equals(rewardVideoID))
        {
            print("Player Should be Rewarded!");
            gameHandler.RespawnPlayer();
        }
    }
}
