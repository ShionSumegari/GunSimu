using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Analytics;
using AppsFlyerSDK;

public class AdsController : MonoBehaviour
{
    [SerializeField] string sdkID;
#if UNITY_ANDROID
    string interAdUnitId = "41231219b2982189";
    string RewardAdUnitId = "93f8d5377e5817b1";
    string bannerAdUnitId = "1d2e72cf661c6da9";
    string appOpenAdUnitId = "68fc8876741cf22e";
#elif UNITY_IOS
    string interAdUnitId = "41231219b2982189";
    string RewardAdUnitId = "93f8d5377e5817b1";
    string bannerAdUnitId = "1d2e72cf661c6da9";
    string appOpenAdUnitId = "68fc8876741cf22e"; 
#else
    string interAdUnitId = "41231219b2982189";
    string RewardAdUnitId = "93f8d5377e5817b1";
    string bannerAdUnitId = "1d2e72cf661c6da9";
    string appOpenAdUnitId = "68fc8876741cf22e"; 
#endif
    int retryAttemptInter, retryAttemptReward;
    public bool AdsIsLoaded = false;
    public bool isFinishedWatchingAds = false;
    public bool isRewardAdsIsReady = false;
    UnityAction RewardedSuccessCallback;

    UnityAction RewardFailedCallback;
    public bool isShowingNormalAd = false;

    //public bool isShowingRewardAd = false;

    public static Action OnRewardAdReload;

    public static Action AdsRewardNotReady;

    public static AdsController Instance;

    public bool IsInitialized;

    string currentAdRewardPlacement = "";

    private void Awake()
    {
        Instance = this;
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {

            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
            InitializeAppOpenAds();

            IsInitialized = true;


            // Show Mediation Debugger
            // MaxSdk.ShowMediationDebugger();
            // AppLovin SDK is initialized, start loading ads
        };

        MaxSdk.SetSdkKey(sdkID);
        MaxSdk.InitializeSdk();
    }


    void Start()
    {

    }

    void InitializeAppOpenAds()
    {
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;

        MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
    }

    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        StartCoroutine(ActionHelper.StartAction(() => isShowingNormalAd = false, 3f));
    }

    public void ShowOAAIfReady()
    {
        if (MaxSdk.IsAppOpenAdReady(appOpenAdUnitId) && !isShowingNormalAd)
        {
            MaxSdk.ShowAppOpenAd(appOpenAdUnitId);
        }
        else
        {
            MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        }
    }

    public bool IsOOAReady()
    {
        return MaxSdk.IsAppOpenAdReady(appOpenAdUnitId);
    }

    #region Interstitial Ads
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(interAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        Debug.Log("INTER ADS IS ALREADY: "+ MaxSdk.IsInterstitialReady(adUnitId) + " "+adInfo.WaterfallInfo);
        AppsFlyerObjectScript.OnInterAdCalled();
        retryAttemptInter = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
        FirebaseManager.Instance?.OnAdsInterLoadedFail(GetInternetStatus()+" "+errorInfo.AdLoadFailureInfo);
        Debug.Log("LogEventAdsInterFail : " + errorInfo.AdLoadFailureInfo + " " + errorInfo.WaterfallInfo.ToString());
        retryAttemptInter++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInter));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        isShowingNormalAd = true;
        AppsFlyerObjectScript.OnInterAdDisplayed();
        FirebaseManager.Instance?.OnAdsInterShow();
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        //Time.timeScale = 0;
    }
    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Interstitial revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!

        FirebaseManager.OnAdRevenuePaidEvent(adInfo);
    }
    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        StartCoroutine(ActionHelper.StartAction(() => isShowingNormalAd = false, 3f));
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        FirebaseManager.Instance?.OnAdsInterFail(GetInternetStatus() + " "+ adInfo.NetworkName + " "+ errorInfo.MediatedNetworkErrorMessage);
        Debug.Log("LogEventAdsInterFail : "+ adInfo.NetworkName + errorInfo.MediatedNetworkErrorMessage + " " + errorInfo.MediatedNetworkErrorCode + " " + errorInfo.WaterfallInfo.ToString());
        Debug.Log("LogEventAdsInterFail WaterFall : "+ errorInfo.WaterfallInfo.Name + " " +errorInfo.WaterfallInfo.NetworkResponses);
        //Time.timeScale = 1;
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        StartCoroutine(ActionHelper.StartAction(() => isShowingNormalAd = false, 3f));
        //Time.timeScale = 1;
        LoadInterstitial();
    }
    public void ShowInterstitial(string placement="")
    {
        if (PlayerPrefs.GetInt("BL_NoInterAds") == 1) return;
        AppsFlyerObjectScript.OnInterAdEligible();
        if (MaxSdk.IsInterstitialReady(interAdUnitId))
        {
            AppsFlyerObjectScript.FireCustomEvent("InterAds" + placement);
            MaxSdk.ShowInterstitial(interAdUnitId);
            Debug.Log("---------SHOW INTER ADS ------------");
        }
    }
    #endregion

    #region Rewarded ads


    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        isRewardAdsIsReady = MaxSdk.IsRewardedAdReady(adUnitId);
        Debug.Log("Reward IS ALREADY: " + isRewardAdsIsReady+ " "+adInfo.WaterfallInfo);
        AppsFlyerObjectScript.OnRewardAdCalled();
        retryAttemptReward = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
        string placementID = currentAdRewardPlacement;
        FirebaseManager.Instance?.OnAdsRewardLoadedFail(GetInternetStatus() + " "+ errorInfo.AdLoadFailureInfo);
        Debug.Log("LogEventAdsRewardFail : " + placementID + "---" + errorInfo.AdLoadFailureInfo + " " + errorInfo.WaterfallInfo.ToString());
        retryAttemptReward++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptReward));

        Invoke("LoadRewardedAd", (float)retryDelay);
        OnRewardAdReload?.Invoke();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppsFlyerObjectScript.OnRewardAdDisplayed();
        FirebaseManager.Instance?.OnAdsRewardShow(currentAdRewardPlacement);
        isShowingNormalAd = true;
        //Time.timeScale = 0;
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        //Time.timeScale = 1;
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        StartCoroutine(ActionHelper.StartAction(() => isShowingNormalAd = false, 3f));
        string placementID = currentAdRewardPlacement;
        FirebaseManager.Instance?.OnAdsRewardFail(GetInternetStatus() + " "+adInfo.NetworkName + " " + placementID,GetInternetStatus() + " "+ errorInfo.MediatedNetworkErrorMessage + " " + errorInfo.MediatedNetworkErrorCode);
        Debug.Log("LogEventAdsRewardFail : " + placementID + "---" + errorInfo.AdLoadFailureInfo + " " + errorInfo.WaterfallInfo.ToString());
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + errorInfo.Code +
          ", description : " + errorInfo.AdLoadFailureInfo);
        LoadRewardedAd();
        OnRewardAdReload?.Invoke();

    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //Time.timeScale = 1;
        // Rewarded ad is hidden. Pre-load the next ad
        StartCoroutine(ActionHelper.StartAction(() => isShowingNormalAd = false, 3f));
        if (isFinishedWatchingAds)
        {
            RewardedSuccessCallback?.Invoke();
        }
        else
        {
            RewardFailedCallback?.Invoke();
        }
        LoadRewardedAd();
        OnRewardAdReload?.Invoke();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        isFinishedWatchingAds = true;
        AppsFlyerObjectScript.OnRewardAdCompleted();
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Rewarded ad revenue paid");

        // Ad revenue
        FirebaseManager.OnAdRevenuePaidEvent(adInfo);
    }
    public void ShowRewardedAd(UnityAction successCallback, UnityAction failCallback, string adPlacement = "")
    {
        if (PlayerPrefs.GetInt("BL_QuickRewardAds") == 1)
        {
            successCallback?.Invoke();
            return;
        }
        AppsFlyerObjectScript.OnRewardAdEligible();
        currentAdRewardPlacement = adPlacement;
        if (MaxSdk.IsRewardedAdReady(RewardAdUnitId))
        {
            MaxSdk.ShowRewardedAd(RewardAdUnitId);
            isFinishedWatchingAds = false;

            RewardedSuccessCallback = successCallback;
            RewardFailedCallback = failCallback;

            AppsFlyerObjectScript.FireCustomEvent("RewardAds" + adPlacement);
        }
        else
        {
            Debug.Log("Show Reward Ads Failed : ads no ready");
            OnRewardAdReload?.Invoke();
        }
    }
    public bool IsRewardAdsReady()
    {
        return MaxSdk.IsRewardedAdReady(RewardAdUnitId);
    }
    public bool IsInterAdsReady()
    {
        return MaxSdk.IsInterstitialReady(interAdUnitId);
    }
    //public bool IsBannerReady()
    //{
    //    return MaxSdk.bann
    //}
    #endregion


    #region Banner Ads

    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        //MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);
        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;


#if !UNITY_EDITOR
        //ShowBanner();
#endif

    }
    public void ShowBanner()
    {
        if (PlayerPrefs.GetInt("BL_NoInterAds") == 1) return;
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        Debug.Log("Bannerer IS loaded");
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
        Debug.Log("Bannerer IS loaded Faild");
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        FirebaseManager.OnAdRevenuePaidEvent(adInfo);
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion

    string GetInternetStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return "NoInternet";
        }
        else
        {
            return "ReachableInternet";
        }
    }

}
