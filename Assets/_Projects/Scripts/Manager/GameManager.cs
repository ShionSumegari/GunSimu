using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeraJet;
using System;

public class GameManager : MonoBehaviour
{

    public AudioManager audioManager;

    public bool firstShowAOA = false;

    public bool isInGame = false;


    Coroutine levelTransCoroutine;

    #region SingleTon
    public static GameManager Instance;

    private void Awake()
    {
        Application.targetFrameRate = 144;
        //effectManager = GetComponent<EffectManager>();
        audioManager = GetComponent<AudioManager>();
        isPlayerDataLoaded = false;
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void VibrationWithDelay(long milliseconds, float timer) // #param1 Duration, #param2 Delay
    {
        StartCoroutine(VibrateDelay(milliseconds, timer));
    }

    IEnumerator VibrateDelay(long milliseconds, float timer)
    {
        yield return new WaitForSeconds(timer);
        Vibration.Vibrate(milliseconds);
    }
    //public void ShowFirstAOA()
    //{
    //    if (!firstShowAOA)
    //    {
    //        firstShowAOA = true;
    //        StartCoroutine(IE_ShowFirstAOA());
    //    }
    //}
    //IEnumerator IE_ShowFirstAOA()
    //{
    //    float waiter = 0;
    //    WaitForSeconds wait = new WaitForSeconds(0.5f);
    //    while (waiter <= 5 && !AdsController.Instance.IsOOAReady())
    //    {
    //        waiter += 0.5f;
    //        yield return wait;
    //    }
    //    if (userData._firstOAA)
    //    {
    //        userData._firstOAA = false;
    //    }
    //    else
    //    {
    //        if (!isInGame)
    //        {
    //            AdsController.Instance.ShowOAAIfReady();
    //        }
    //    }

    //}
    #region LOAD_USER_DATA
    public event System.Action OnPlayerDataLoaded;

    [SerializeField] UserData m_PlayerData;

    public UserData userData { get { return m_PlayerData; } set { m_PlayerData = value; } }

    bool m_IsDataLoaded;

    public bool isPlayerDataLoaded { get { return m_IsDataLoaded; } set { m_IsDataLoaded = value; LoadData(); } }

    //public double PlayerMoney
    //{
    //    get { return m_PlayerData._currentMoney; }
    //    set
    //    {
    //        m_PlayerData._currentMoney = value;
    //        if (OnMoneyChange != null)
    //        {
    //            OnMoneyChange.Invoke(value);
    //        }
    //    }
    //}

    public static System.Action<double> OnMoneyChange = delegate { };

    public void LoadData()
    {
        if (!m_IsDataLoaded)
        {
            StartCoroutine(LoadUserData());
        }
        else
        {
            QualitySettings.SetQualityLevel(m_PlayerData._qualitySettingsIndex);
            if (OnPlayerDataLoaded != null)
            {
                LogUtils.Log("Fire event current playerDataLoaded !!!");
                OnPlayerDataLoaded();
            }
        }
    }

    IEnumerator LoadUserData()
    {
        if (!isPlayerDataLoaded)
        {
            m_PlayerData = SaveSystem.LoadPlayer();
        }
        while (m_PlayerData == null)
        {
            LogUtils.Log("Waiting for Player Data !!!");
            yield return null;
        }
        UpdateUserSettingsData();
        m_IsDataLoaded = true;

        if (OnPlayerDataLoaded != null)
        {
            LogUtils.Log("Fire event playerDataLoaded for other objects !!!! ");
            OnPlayerDataLoaded();
        }
    }

    private void UpdateUserSettingsData()
    {
        QualitySettings.SetQualityLevel(m_PlayerData._qualitySettingsIndex);
    }
    #endregion

    public void ResetAllData()
    {
        userData = new UserData();
        SaveSystem.SavePlayer(userData);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSystem.SavePlayer(userData);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && !isInGame)
        {
            //AdsController.Instance.ShowOAAIfReady();
        }
        if (pause)
        {
            SaveSystem.SavePlayer(userData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SavePlayer(userData);
    }
}
