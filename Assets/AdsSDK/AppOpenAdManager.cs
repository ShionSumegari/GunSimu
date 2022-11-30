using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;

public class AppOpenAdManager : Singleton<AppOpenAdManager>
{
#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5662855259";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif
    private DateTime loadTime;

    private bool hasShownAOA = false;

    private bool isShowing;

    AppOpenAd appOpenAd;
    protected override void Awake()
    {
        base.Awake();
        MobileAds.SetiOSAppPauseOnBackground(true);
        if(PlayerPrefs.GetInt("BL_IsTestAds", 0) == 1)
        {
            List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };
#if UNITY_ANDROID
            string testDeviceID = PlayerPrefs.GetString("BL_TestDeviceID", "");
            deviceIds.Add(testDeviceID);
            //deviceIds.Add("646a8a7d914d4f72aa2c80023afaee4c");
#endif
            ///LoadAd();
            RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
        
    }
    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.

        Debug.Log("Initialization complete");
        LoadAd();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }
    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        //UnityEngine.Debug.Log("App State is " + state);
        //if (state == AppState.Foreground)
        //{
        //    AppOpenAdManager.Instance.ShowAdIfAvailable();
        //}
    }

    public bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null;
        }
    }

    public void LoadAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        Debug.Log("Load AOA Ads");
        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(AD_UNIT_ID, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                return;
            }

            Debug.Log("AppOpenAd loaded. Please background the app and return.");

            // App open ad is loaded.
            this.appOpenAd = appOpenAd;
        }));

    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    public void ShowAdIfAvailable()
    {
        if (!IsAdAvailable || isShowing || AdsController.Instance.isShowingNormalAd)
        {

            Debug.Log("AOA is not available - reason IsAdAvai" + IsAdAvailable + "-hasShown " + hasShownAOA + "-showingnormal "+ AdsController.Instance.isShowingNormalAd);
            //LoadAd();
            return;
        }

        if(GameManager.Instance == null || !GameManager.Instance.firstShowAOA)
        {
            Debug.Log("Gamehandle: " + GameManager.Instance);
            return;
        }

        Debug.Log("Display AOA");
        this.appOpenAd.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        this.appOpenAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        this.appOpenAd.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        this.appOpenAd.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        this.appOpenAd.OnPaidEvent += HandlePaidEvent;
        appOpenAd.Show();

    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");

        StartCoroutine(ActionHelper.StartAction(() => { isShowing = false; }, 3f));

        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        this.appOpenAd.OnAdDidDismissFullScreenContent -= HandleAdDidDismissFullScreenContent;
        this.appOpenAd.OnAdFailedToPresentFullScreenContent -= HandleAdFailedToPresentFullScreenContent;
        this.appOpenAd.OnAdDidPresentFullScreenContent -= HandleAdDidPresentFullScreenContent;
        this.appOpenAd.OnAdDidRecordImpression -= HandleAdDidRecordImpression;
        this.appOpenAd.OnPaidEvent -= HandlePaidEvent;
        this.appOpenAd = null;
        LoadAd();
    }
    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        this.appOpenAd.OnAdDidDismissFullScreenContent -= HandleAdDidDismissFullScreenContent;
        this.appOpenAd.OnAdFailedToPresentFullScreenContent -= HandleAdFailedToPresentFullScreenContent;
        this.appOpenAd.OnAdDidPresentFullScreenContent -= HandleAdDidPresentFullScreenContent;
        this.appOpenAd.OnAdDidRecordImpression -= HandleAdDidRecordImpression;
        this.appOpenAd.OnPaidEvent -= HandlePaidEvent;
        this.appOpenAd = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowing = true;
        AppsFlyerObjectScript.OnAppOpenAdsDisplayed();
        //isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
        FirebaseManager.OnAOAPaidEvent(args.AdValue);
    }
}
