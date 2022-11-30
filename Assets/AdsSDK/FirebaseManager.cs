using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using System;

public class FirebaseManager : Singleton<FirebaseManager>
{
    [SerializeField] int m_revenueDayInterval = 1;
    public bool isInitialized;
    void Start()
    {

    }
    public void Initialize()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase Initialized");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }
    protected void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        // Set a flag here to indicate whether Firebase is ready to use by your app.
        isInitialized = true;
    }
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    public void OnDestroy()
    {
        if (isInitialized)
        {
            
            Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
        }
    }
    //Firebase Normal Event

    public void SendNormalEvent(string eventKey, params Parameter[] parameters)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.LogEvent(eventKey, parameters);
    }
    public void OnLevelStartEvent(string currentMoneyVal)
    {
        SendNormalEvent("level_start", new Parameter("current_gold", currentMoneyVal));
    }
    public void OnAdsRewardShow(string placement)
    {
        SendNormalEvent("ads_reward_show", new Parameter("placement", placement));
    }
    public void OnAdsRewardFail(string placement, string errorMsg)
    {
        SendNormalEvent("ads_reward_fail", new Parameter("placement", placement), new Parameter("errormsg", errorMsg));
    }
    public void OnAdsRewardLoadedFail(string errorMsg)
    {
        SendNormalEvent("ads_reward_loaded_fail", new Parameter("errormsg", errorMsg));
    }
    public void OnAdsInterFail(string errorMsg)
    {
        SendNormalEvent("ad_inter_fail", new Parameter("errormsg", errorMsg));
    }
    public void OnAdsInterLoadedFail(string errorMsg)
    {
        SendNormalEvent("ads_inter_loaded_fail", new Parameter("errormsg", errorMsg));
    }
    public void OnAdsInterShow()
    {
        SendNormalEvent("ad_inter_show", new Parameter("placement", ""));
    }

    //Firebase Ad impression
    //public static void OnAOAPaidEvent(GoogleMobileAds.Api.AdValue adValue)
    //{
    //    double revenue = adValue.Value;
    //    var impressionParameters = new[] {
    //        new Firebase.Analytics.Parameter("ad_platform", "AdMob"),
    //        new Firebase.Analytics.Parameter("ad_source", "AdMob"),
    //        new Firebase.Analytics.Parameter("ad_unit_name", ""),
    //        new Firebase.Analytics.Parameter("ad_format", "AOA"),
    //        new Firebase.Analytics.Parameter("value", revenue),
    //        new Firebase.Analytics.Parameter("country_code",MaxSdk.GetSdkConfiguration().CountryCode),
    //        new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode), // All AppLovin revenue is sent in USD
    //     };
    //    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    //}
    public static void OnAdRevenuePaidEvent(MaxSdkBase.AdInfo impressionData)
    {
        SendNormalAdsImpression(impressionData);
    }
    static void SendNormalAdsImpression(MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        var impressionParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
            new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
            new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
            new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("country_code",MaxSdk.GetSdkConfiguration().CountryCode),
            new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
         };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    }
    double minRevToSent = 1.0f;
    void SendLTVD1AdsImpression(MaxSdkBase.AdInfo impressionData)
    {
        double totalLTVRev = impressionData.Revenue + GetLTVD1Rev();

        //if(totalLTVRev >= minRevToSent)
    }
    public bool CheckValidDay(string lastCheckTime, int dayInterval)
    {
        try
        {
            DateTime currentDate = DateTime.Now;
            DateTime lastCheckDate = new DateTime();

            bool isParsed = DateTime.TryParse(lastCheckTime, out lastCheckDate);

            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            lastCheckDate = new DateTime(lastCheckDate.Year, lastCheckDate.Month, lastCheckDate.Day, 0, 0, 0);

            if (!isParsed)
            {
                return true;
            }

            double totalDays = (lastCheckDate - currentDate).TotalDays;

            if(totalDays >= dayInterval)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return true;
        }
    }
    string GetLTVD1LastCheckTime()
    {
        return PlayerPrefs.GetString("LVTD1_LastCheckTIme", DateTime.Now.ToString());
    }
    void SetLTVD1LastCheckTime(string time)
    {
        PlayerPrefs.SetString("LVTD1_LastCheckTIme", time);
    }
    double GetLTVD1Rev()
    {
        return (double)PlayerPrefs.GetFloat("LVTD1_Rev", 0);
    }
    void SetLTVD1Rev(double value)
    {
        PlayerPrefs.SetFloat("LVTD1_Rev", (float)value);
    }
}
