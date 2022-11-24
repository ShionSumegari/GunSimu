using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace TeraJet
{
#if UNITY_EDITOR
    public class TeraGameEditor : EditorWindow
    {
        string m_NotificationTitle = "We miss you, hero";
        string m_NotificationDesc = "Come back!";
        string m_Button = "Save";
        bool groupEnabled;
        bool m_IsInternetConnectRequired = false;

        // Add menu named "TeraGameConfig" to the Window menu
        [MenuItem("Assets/Tera Game Config")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TeraGameEditor window = (TeraGameEditor)EditorWindow.GetWindow(typeof(TeraGameEditor));
            window.Show();
        }

        private void OnEnable()
        {
            LoadDataFromDB();
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            m_NotificationTitle = EditorGUILayout.TextField("Notification Title", m_NotificationTitle);
            m_NotificationDesc = EditorGUILayout.TextField("Notification Description", m_NotificationDesc);

            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            m_IsInternetConnectRequired = EditorGUILayout.Toggle("Internet Connection Required", m_IsInternetConnectRequired);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button(m_Button))
            {
                SaveDataToDB();
            }
        }

        void LoadDataFromDB()
        {
            TeraGameConfig teraGameConfig = GameUtils.LoadSDKSettings();
            if (teraGameConfig != null)
            {
                m_NotificationTitle = teraGameConfig.notificationTitle;
                m_NotificationDesc = teraGameConfig.notificationDesc;
                m_IsInternetConnectRequired = teraGameConfig.isInternetConnectionRequired;
            }
            else
            {
                Debug.Log("Teraconfig can't be loaded");
            }
        }

        void SaveDataToDB()
        {
            TeraGameConfig teraGameConfig = GameUtils.LoadSDKSettings();

            teraGameConfig.notificationDesc = m_NotificationDesc;
            teraGameConfig.notificationTitle = m_NotificationTitle;
            teraGameConfig.isInternetConnectionRequired = m_IsInternetConnectRequired;

            GameUtils.SaveSDKSettings(teraGameConfig);
            GameUtils.staticTeraGameConfig = teraGameConfig;
            Debug.Log("Static Game Config: " + GameUtils.staticTeraGameConfig.isInternetConnectionRequired);

            
        }
    }
#endif
}

