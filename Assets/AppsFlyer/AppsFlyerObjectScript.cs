using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

// This class is intended to be used the the AppsFlyerObject.prefab

public class AppsFlyerObjectScript : MonoBehaviour , IAppsFlyerConversionData
{

    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public bool isDebug;
    public bool getConversionData;
    //******************************//

    public static AppsFlyerObjectScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // These fields are set from the editor so do not modify!
        //******************************//
        AppsFlyer.setIsDebug(isDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
        AppsFlyer.initSDK(devKey, UWPAppID, getConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
    AppsFlyer.initSDK(devKey, macOSAppID, getConversionData ? this : null);
#else
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
#endif
        //******************************/
 
        AppsFlyer.startSDK();
    }


    void Update()
    {

    }
    // Inter ads event
    public static void OnInterAdEligible()
    {
        AppsFlyer.sendEvent("af_inters_ad_eligible", null);
    }
    public static void OnInterAdCalled()
    {
        AppsFlyer.sendEvent("af_inters_api_called", null);
    }
    public static void OnInterAdDisplayed()
    {
        AppsFlyer.sendEvent("af_inters_displayed", null);
    }

    //Reward ads event

    public static void OnRewardAdEligible()
    {
        AppsFlyer.sendEvent("af_rewarded_ad_eligible", null);
    }
    public static void OnRewardAdCalled()
    {
        AppsFlyer.sendEvent("af_rewarded_api_called", null);
    }
    public static void OnRewardAdDisplayed()
    {
        AppsFlyer.sendEvent("af_rewarded_displayed", null);
    }
    public static void OnRewardAdCompleted()
    {
        AppsFlyer.sendEvent("af_rewarded_ad_completed", null);
    }

    public static void FireCustomEvent(string e)
    {
        AppsFlyer.sendEvent(e, null);
    }

    public static void OnAppOpenAdsDisplayed()
    {
        AppsFlyer.sendEvent("af_aoa_ad_displayed", null);
    }


    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

}
