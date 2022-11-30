using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Project.Cheat
{
    public class ConfigPanel : CheatPanelElement
    {
        const string Key_NoInter = "BL_NoInterAds";
        const string Key_QuickReward = "BL_QuickRewardAds";
        const string Key_IsTestAds = "BL_IsTestAds";
        const string Key_IsDebug = "BL_IsDebug";
        const string Key_LogcatEnable = "BL_LogcatEnable";

        [SerializeField] Toggle _interToggle;
        [SerializeField] Toggle _rewardToggle;
        [SerializeField] Toggle _testAdsToggle;
        [SerializeField] Toggle _debugToggle;
        [SerializeField] Toggle _logcatToggle;

        [SerializeField] TMP_InputField _testDeviceIDInput;
        [SerializeField] Button _btnSaveIDInput;

        public override void OnInitialize()
        {
            _interToggle.isOn = PlayerPrefs.GetInt(Key_NoInter, 0) == 1;
            _rewardToggle.isOn = PlayerPrefs.GetInt(Key_QuickReward, 0) == 1;
            _testAdsToggle.isOn = PlayerPrefs.GetInt(Key_IsTestAds, 0) == 1;
            _debugToggle.isOn = PlayerPrefs.GetInt(Key_IsDebug, 0) == 1;
            _logcatToggle.isOn = PlayerPrefs.GetInt(Key_LogcatEnable, 0) == 1;

            _testDeviceIDInput.text = PlayerPrefs.GetString("BL_TestDeviceID", "");

#if DebugConsole
            if(IngameDebugConsole.DebugLogManager.Instance!=null){
                IngameDebugConsole.DebugLogManager.Instance.PopupEnabled = _debugToggle.isOn;
            } 
#endif

            _interToggle.onValueChanged.AddListener(OnInterToggleChange);
            _rewardToggle.onValueChanged.AddListener(OnRewardToggleChange);
            _testAdsToggle.onValueChanged.AddListener(OnTestAdsToggleChange);
            _debugToggle.onValueChanged.AddListener(OnDebugToggleChange);
            _logcatToggle.onValueChanged.AddListener(OnLogcatToggleChange);
            _btnSaveIDInput.onClick.AddListener(SaveTestDeviceID);
        }
        public void SaveTestDeviceID()
        {
            PlayerPrefs.SetString("BL_TestDeviceID", _testDeviceIDInput.text);
        }
        protected void OnInterToggleChange(bool isOn)
        {
            int value = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Key_NoInter, value);
        }
        protected void OnRewardToggleChange(bool isOn)
        {
            int value = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Key_QuickReward, value);
        }
        protected void OnTestAdsToggleChange(bool isOn)
        {
            int value = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Key_IsTestAds, value);
        }
        protected void OnDebugToggleChange(bool isOn)
        {
            int value = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Key_IsDebug, value);

#if DebugConsole
            if(IngameDebugConsole.DebugLogManager.Instance!=null){
                IngameDebugConsole.DebugLogManager.Instance.PopupEnabled = _debugToggle.isOn;
            } 
#endif
        }
        protected void OnLogcatToggleChange(bool isOn)
        {
            int value = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Key_LogcatEnable, value);
        }
    }
}
