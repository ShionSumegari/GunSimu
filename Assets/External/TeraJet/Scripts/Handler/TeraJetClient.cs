using UnityEngine;
using UnityEditor;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

namespace TeraJet
{
    public static class TeraJetClient
    {
        public static System.Action<GameAdData> OnAdLoaded;
        public static System.Action OnAdLoadFailed;

        public static GameAdData currentAppData;

        private static RequestHelper currentRequest;

        private static void LogMessage(string title, string message)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(title, message, "OK");
#elif UNITY_ANDROID
		Debug.Log(message);
#else
            Debug.Log(message);
#endif
        }

        private static string _platform;
        public static string platform { set {  _platform = value; } get
            {
                if (!string.IsNullOrEmpty(_platform)) return _platform;

#if UNITY_IOS
                return PlayerPrefsConfig.PLATFORM_IOS;
#elif UNITY_ANDROID
                return PlayerPrefsConfig.PLATFORM_ANDROID;
#else
                return PlayerPrefsConfig.PLATFORM_ANDROID;
#endif
            }
        }


        private static int _versionCode;
        public static int versionCode
        {
            set { _versionCode = value; }
            get
            {
                if (!string.IsNullOrEmpty(_platform)) return _versionCode;

#if UNITY_IOS
                return GameUtils.PLATFORM_IOS;
#elif UNITY_ANDROID && !UNITY_EDITOR
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
                var pInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);
                return pInfo.Get<int>("versionCode");
#else
                return PlayerSettings.Android.bundleVersionCode;
#endif
            }
        }


        public static void Initialize()
        {
            TeraGameConfig teraGameConfig = GameUtils.LoadSDKSettings();
            
            if (ConnectionHandler.CheckForInternetConnection())
            {
                GetAppConfig();
                
            } else
            {
                if (teraGameConfig != null && teraGameConfig.isInternetConnectionRequired)
                {
                    GameUtils.ShowDialog(PopupController.PopupType.CONNECTION_REQUIRED);
                }
            }

            NotificationHandler.NotificationInitialize();
            

        }

        /***********************************************************************
         * Request Ads Config from RESTful Server                                       
         ***********************************************************************/
        public static void GetAppConfig()
        {
            currentAppData = null;
            currentRequest = new RequestHelper
            {
                Uri = TeraJetServerConfig.BASE_PATH + TeraJetServerConfig.GAME_ADS_CONFIG_PATH,
                Body = new BasePost
                {
                    package_name = Application.identifier,
                    platform_name = platform,
                },
                EnableDebug = true
            };
            RestClient.Post<GameAdResponse>(currentRequest)
            .Then(res => {
                BasePost pos = (BasePost)currentRequest.Body;
                Debug.Log("Body: " + pos.ToString());
                // And later we can clear the default query string params for all requests
                RestClient.ClearDefaultParams();
                LogMessage("Success", JsonUtility.ToJson(res, true));
                if(res.code == TeraJetServerConfig.SUCCESS_CODE)
                {
                    currentAppData = res.data;

                    if (res.data.version_code != 0 && res.data.version_code != versionCode)
                    {
                        if(TerajetManager.Instance != null)
                        {
                            TerajetManager.Instance.ShowUpdatePopup();
                        }
                    }
                    if (OnAdLoaded != null) OnAdLoaded(currentAppData);
                }
                else
                {
                    if (OnAdLoadFailed != null) OnAdLoadFailed();
                }
            })
            .Catch(err => {
                if (OnAdLoadFailed != null) OnAdLoadFailed();
                LogMessage("Error", err.Message);
            });
        }
    }
}


