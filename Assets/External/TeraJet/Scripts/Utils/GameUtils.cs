using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEditor;
using Proyecto26;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace TeraJet
{
    public static class GameUtils
    {

        public static TeraGameConfig staticTeraGameConfig = new TeraGameConfig();

        public static PopupController.PopupType m_PopupType;

        #region PLAYER_DATA

        public static void SavePlayerData(PlayerData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + PlayerPrefsConfig.PLAYER_DATA;

            FileStream stream = new FileStream(path, FileMode.Create);

            // PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData LoadPlayerData()
        {
            string path = Application.persistentDataPath + PlayerPrefsConfig.PLAYER_DATA;
            if (File.Exists(path))
            {
                Debug.Log("Saved file found in " + path);

                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogWarning("Saved file not found in " + path + " Generate Zero Data");
                PlayerData data = new PlayerData();
                return data;
            }
        }

        #endregion

        #region UTILS

        public static T[] AddItemToArray<T>(this T[] original, T itemToAdd)
        {
            T[] finalArray = new T[original.Length + 1];
            for (int i = 0; i < original.Length; i++)
            {
                finalArray[i] = original[i];
            }
            finalArray[finalArray.Length - 1] = itemToAdd;
            return finalArray;
        }

        public static T[] Reverse<T>(T[] array)
        {
            var result = new T[array.Length];
            int j = 0;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                result[j] = array[i];
                j++;
            }
            return result;
        }

        public static string GetRuntimeInternetStatus()
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
        public static void ScaleIn(GameObject inGameObject)
        {

        }

        static IEnumerator ScaleInAnim(GameObject inGameObject)
        {
            Vector3 scale = inGameObject.transform.localScale;
            inGameObject.transform.localScale = Vector3.zero;
            while (inGameObject.transform.localScale.magnitude < scale.magnitude)
            {
                float s = 5f * Time.unscaledDeltaTime;
                inGameObject.transform.localScale += new Vector3(s, s, s);
                yield return new WaitForEndOfFrame();
            }
            inGameObject.transform.localScale = scale;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        #endregion

        #region EXCUTE_FUNCTION

        public delegate void FunctionType();

        public static FunctionType excuteFunction { set { excuteFunction = value; ExcuteFunction(value, 0f); } }

        public static void ExcuteFunction(FunctionType function, float delayTime)
        {
            TerajetManager.Instance.StartCoroutine(ExcuteFunctionIE(function, delayTime));
        }

        static IEnumerator ExcuteFunctionIE(FunctionType function, float time)
        {
            yield return new WaitForSeconds(time);
            function();
        }

        #endregion

        #region REQUEST_PERMISSIONS

        public static void RequestPermission(MonoBehaviour monoBehaviour)
        {

        }

        #endregion

        #region POPUP_SHOWING

        public static void ShowDialog(PopupController.PopupType type)
        {
            GameObject popCanvas = GameObject.Instantiate(Resources.Load("UIs/UICanvas", typeof(GameObject)) as GameObject);
            PopupController popupController;
            popupController = popCanvas.GetComponentInChildren<PopupController>();
            popupController.m_Type = type;
            if (type == PopupController.PopupType.RATE)
            {
#if UNITY_ANDROID
                return;
#elif UNITY_IOS
        Device.RequestStoreReview();
#endif
            }
        }


        public static void OpenAppStoreGame(string gameId)
        {
#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + Application.identifier);
#endif
        }

        #endregion

        #region SDK_SETTINGS
        public static void SaveSDKSettings(TeraGameConfig data)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + PlayerPrefsConfig.TERA_SETTINGS;

            FileStream stream = new FileStream(path, FileMode.Create);

            TeraGameConfig newData = new TeraGameConfig(data);

            formatter.Serialize(stream, newData);
            stream.Close();
        }

        public static TeraGameConfig LoadSDKSettings()
        {
            string path = Application.persistentDataPath + PlayerPrefsConfig.TERA_SETTINGS;
            if (File.Exists(path))
            {
                Debug.Log("Saved file found in " + path);

                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(path, FileMode.Open);

                TeraGameConfig data = formatter.Deserialize(stream) as TeraGameConfig;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogWarning("Saved file not found in " + path + " Generate Zero Data");
                TeraGameConfig data = new TeraGameConfig();
                TeraGameConfigSO teraGameConfigSO = (TeraGameConfigSO) Resources.Load("TeraGameConfig", typeof(TeraGameConfigSO));

                if(teraGameConfigSO != null)
                {
                    data.notificationTitle = teraGameConfigSO.notificationTitle;
                    data.isInternetConnectionRequired = teraGameConfigSO.isInternetConnectionRequired;
                    data.notificationDesc = teraGameConfigSO.notificationDesc;
                    Debug.Log("Static config: " + teraGameConfigSO.isInternetConnectionRequired);
                }
                
                return data;
            }
        }

        #endregion
    }
}
