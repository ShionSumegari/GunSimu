using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdsManager : Singleton<AdsManager>
{
    public enum AdState
    {
        Ready,
        NotReady,
    }
    bool isAdReady;

    [SerializeField] float m_interAdsInterval = 120f;

    float m_interAdsTimer;

    bool canShowInter = false;

    public static System.Action OnRewardAdsLoaded = delegate { };
    public static System.Action OnRewardAdsNotLoaded = delegate { };

    Coroutine coroutineWaitForAdsLoaded;

    public System.Action OnRewardSpeed;

    protected override void Awake()
    {
        base.Awake();
        AdsController.OnRewardAdReload += OnRewardAdsOver;
        m_interAdsTimer = m_interAdsInterval;
    }
    private void OnDestroy()
    {
        AdsController.OnRewardAdReload -= OnRewardAdsOver;
    }
    private void Start()
    {
        CheckRewardLoaded();
        //StartCoroutine(ShowInterAdsRoutine());
    }

    int showCount = 0;

    IEnumerator ShowInterAdsRoutine()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            if (!canShowInter)
            {
                if(m_interAdsTimer <= 0)
                {
                    canShowInter = true;
                }
                m_interAdsTimer -= Time.deltaTime;
            }
            if(canShowInter && AdsController.Instance.IsInterAdsReady() && !AdsController.Instance.isShowingNormalAd)
            {
                AdsController.Instance.ShowInterstitial();
                showCount++;
                canShowInter = false;
                m_interAdsTimer = Mathf.Clamp(m_interAdsInterval - showCount*10,70f,120f);
            }
            yield return waitForEndOfFrame;
        }
    }
    public void ShowInterAds(string placement = "")
    {
        if (AdsController.Instance.IsInterAdsReady() && !AdsController.Instance.isShowingNormalAd)
        {
            AdsController.Instance.ShowInterstitial(placement);
        }
    }
    void CheckRewardLoaded()
    {
        bool isAdLoaded = AdsController.Instance.IsRewardAdsReady();
        if (isAdLoaded)
        {
            OnRewardAdsLoaded.Invoke();
        }
        else
        {
            OnRewardAdsNotLoaded.Invoke();

            if (coroutineWaitForAdsLoaded != null)
            {
                StopCoroutine(coroutineWaitForAdsLoaded);
            }
            coroutineWaitForAdsLoaded = StartCoroutine(WaitForRewardAdsReady());
        }
    }
    void OnRewardAdsOver()
    {
        CheckRewardLoaded();
    }
    IEnumerator WaitForRewardAdsReady()
    {
        bool isAdLoaded = AdsController.Instance.IsRewardAdsReady();
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (isAdLoaded == false)
        {
            isAdLoaded = AdsController.Instance.IsRewardAdsReady();
            yield return wait;
        }

        OnRewardAdsLoaded.Invoke();
    }
}
public interface IHuman
{
    public void Burn(float hit);

    public void OnDamage(int damage);

    public void Eat();
}

public interface IBurnable
{
    public void Burn(float hit);
}

public interface IHealth
{
    public void OnDamage(int damage);
}
