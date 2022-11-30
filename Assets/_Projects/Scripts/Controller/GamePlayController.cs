using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GamePlayController : MonoBehaviour
{
    [Header("Gun State:")]
    public bool isSingleShot;
    public bool isBurtMode;
    public bool isAuto;
    public bool isShake;

    [Header("Other State:")]
    [SerializeField] bool isUntimateBullet;
    [SerializeField] public bool isFlashLight;
    [SerializeField] public bool isVibrate;
    [SerializeField] public bool isVFX;
    [SerializeField] public bool isZoom;
    [SerializeField] GameObject smoke;
    public bool isRotate;
    [SerializeField] RectTransform rotateButton;

    [Header("OTHER STATE IMAGE:")]
    [SerializeField] GameObject imageInfi; 
    [SerializeField] GameObject imageLight; 
    [SerializeField] GameObject imageVibrate; 
    [SerializeField] GameObject imageVFX; 
    [SerializeField] GameObject imageZoomIn; 
    [SerializeField] GameObject imageZoomOut; 

    [Header("Controler: ")]
    public bool isCanShoot;
    private bool isStartTouchFail;

    [Header("Bullet")]
    public int bulletamount;
    public Transform bulletHolder;

    [Header("Other Settings:")]
    [SerializeField] GameObject gunHolder;
    [SerializeField] bool isOutOfBullet;

    [Header("Reload Group:")]
    [SerializeField] GameObject swipe;
    [SerializeField] GameObject reload;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletHolderGun;
    [SerializeField] public GameObject reloadTime;
    [SerializeField] Image fillTime;
    [SerializeField] Text textTime;

    Vector3 startPos = Vector3.zero;
    Vector3 endPos = Vector3.zero;
    public float delayShot;
    public float delayHold;
    private float timeDelayCounter = 0;
    [SerializeField] bool timeReload;
    [SerializeField] RectTransform hideUI;
    [SerializeField] RectTransform zoomUI;
    [SerializeField] public Image gunImage;
    Coroutine coroutine;
    Coroutine coroutineBurst;
    bool isReloaded;
    float delay = 0;
    bool justHold;
    float reloadTimer;
    public bool isShakeMode;
    public bool isZoomFullScreen;
    #region SingleTon
    public static GamePlayController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public void Update()
    {
        if (isSingleShot)
        {
            SingleShotAndBurtModeGun(false);
        }
        if (isBurtMode)
        {
            SingleShotAndBurtModeGun(true);
        }
        if (isAuto)
        {
            HoldGun();
        }

        // Reload Gun
        if (!isUntimateBullet)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPos = Input.GetTouch(0).position;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && isOutOfBullet && bulletHolder.childCount == 0)
            {
                endPos = Input.GetTouch(0).position;
                if (Vector3.Distance(startPos, endPos) >= 150)
                {
                    StartCoroutine(ReLoadGun(false));
                }
            }
        }
        else if(isUntimateBullet)
        {
            if (isOutOfBullet)
            {
                StartCoroutine(ReLoadGun(true));
            }
        }
    }

    #region SHOT_STATE
    public void SingleShotAndBurtModeGun(bool isBurstMode)
    {
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && !isCanShoot)
            {
                isStartTouchFail = true;
                return;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended && isCanShoot && !isStartTouchFail)
            {
                if (!isBurstMode)
                {
                    if (!isOutOfBullet)
                    {
                        gunHolder.GetComponent<GunDecoil>().DeCoilGun();
                    }
                    BulletSetUp();
                }
                else if (isBurstMode)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                   coroutine =  StartCoroutine(WaitBurst());
                }
            }
        }
        else
        {
            isStartTouchFail = false;
        }
    }

    public void HoldGun()
    {
        if(Input.touchCount > 0)
        {
            if((Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved) && isCanShoot)
            {     
                if(delay < 0.05f)
                {
                    delay += Time.deltaTime;
                    return;
                }
                if (timeDelayCounter < delayHold)
                {
                    timeDelayCounter += Time.deltaTime;
                }
                else
                {
                    if (!isOutOfBullet)
                    {
                        justHold = true;
                        gunHolder.GetComponent<GunDecoil>().DeCoilGun();
                    }
                    BulletSetUp();
                    timeDelayCounter = 0;
                }
            }
            else 
            {
                if (justHold)
                {
                    justHold = false;
                    return;
                }
                if (!isOutOfBullet && isCanShoot && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    gunHolder.GetComponent<GunDecoil>().DeCoilGun();
                    BulletSetUp();
                }
            }
        }
        else { delay = 0; }
    }

    public void ShakeGun()
    {
        if (timeDelayCounter < delayHold)
        {
            timeDelayCounter += Time.deltaTime;
        }
        else
        {
            if (!isOutOfBullet)
            {
                justHold = true;
                gunHolder.GetComponent<GunDecoil>().DeCoilGun();
            }
            BulletSetUp();
            timeDelayCounter = 0;
        }
    }

    IEnumerator WaitBurst()
    {
        yield return new WaitUntil(() => !isShakeMode);
        if (coroutineBurst != null)
        {
            StopCoroutine(coroutineBurst);
        }
        coroutineBurst = StartCoroutine(BurstMode());
    }
    IEnumerator BurstMode()
    {
        for(int i = 1; i<= GameplayUIController.Instance.burstMode; i++)
        {
            isShakeMode = true;
            if (!isOutOfBullet)
            {
                gunHolder.GetComponent<GunDecoil>().DeCoilGun();
            }
            BulletSetUp();
            yield return new WaitForSeconds(delayShot);
            isShakeMode = false;
        }
    }


    void BulletSetUp()
    {
        if(bulletHolder.childCount <= 51)
        {
            GameplayUIController.Instance.bulletHolder.gameObject.SetActive(true);
            GameplayUIController.Instance.overBulletHolder.gameObject.SetActive(false);
        }
        if(bulletHolder.childCount > 0)
        {
            Destroy(bulletHolder.GetChild(0).gameObject);
            GameplayUIController.Instance.bulletText.text = "X " + (bulletHolder.childCount -1);
        } else if(bulletHolder.childCount == 0 && !timeReload)
        {
            GameManager.Instance.audioManager.PlaySFX("EmtyGun", 1f);
        }
        if(bulletHolder.childCount == 1)
        {
            swipe.SetActive(true);
            reload.SetActive(true);
            isOutOfBullet = true;
            isReloaded = false;
        }
    }
    
    public void SwipeClose()
    {
        swipe.SetActive(false);
        reload.SetActive(false);
        isOutOfBullet = false;
    }
    IEnumerator ReLoadGun(bool isUltimate)
    {
        reloadTimer = GameplayUIController.Instance.reloadTime;
        if (!isReloaded)
        {
            isReloaded = true;
            timeReload = true;
            swipe.SetActive(false);
            reload.SetActive(false);
            if (!isUntimateBullet)
            {
                reloadTime.SetActive(true);
                gunImage.color = new Color32(75, 75, 75, 255);
                GameManager.Instance.audioManager.StopSFX("EmtyGun", 1f);
                GameManager.Instance.audioManager.PlaySFX(gunHolder.GetComponent<GunDecoil>().gunName.text + "R", 1f);
                fillTime.DOFillAmount(0, GameplayUIController.Instance.reloadTime).SetEase(Ease.Linear).OnUpdate(()=> {
                    reloadTimer -= Time.deltaTime;
                    textTime.text = Math.Round(reloadTimer, 1).ToString();
                }).OnComplete(()=> {
                    reloadTime.SetActive(false);
                    gunImage.color = new Color32(255, 255, 255, 255);
                    fillTime.fillAmount = 1;
                });
                yield return new WaitForSeconds(GameplayUIController.Instance.reloadTime);
            }
            yield return null;
            foreach(Transform tr in bulletHolderGun)
            {
                Destroy(tr.gameObject);
            }
            for (int i = 0; i < bulletamount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletHolderGun);
                bullet.GetComponent<Image>().sprite = GameplayUIController.Instance.targetImgBullet;
            }
            GameplayUIController.Instance.bulletText.text = "X " + bulletamount;
            isOutOfBullet = false;
            timeReload = false;
            if (bulletamount > 50)
            {
                GameplayUIController.Instance.bulletHolder.gameObject.SetActive(false);
                GameplayUIController.Instance.overBulletHolder.gameObject.SetActive(true);
            }
            else
            {
                GameplayUIController.Instance.bulletHolder.gameObject.SetActive(true);
                GameplayUIController.Instance.overBulletHolder.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region OTHER_STATE
    public void _OnClickRotateButton()
    {
        GameManager.Instance.audioManager.PlaySFX("Switch", 1f);
        if (!isRotate)
        {
            gunHolder.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, -180, 0);
            smoke.transform.localPosition = new Vector3(20, 0, 400);
        }
        if (isRotate)
        {
            gunHolder.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
            smoke.transform.localPosition = new Vector3(20, 0, -400);
        }
        isRotate = !isRotate;
    }
    public void _OnClickUntimaeButton()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        if (!isUntimateBullet)
        {
            imageInfi.SetActive(true);
        }
        else
        {
            imageInfi.SetActive(false);
        }
        isUntimateBullet = !isUntimateBullet;
    }

    public void _OnClickLightButton()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        if (!isFlashLight)
        {
            imageLight.SetActive(true);
        }
        else
        {
            imageLight.SetActive(false);
        }
        isFlashLight = !isFlashLight;
    }

    public void _OnClickVibrateButton()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        if (!isVibrate)
        {
            imageVibrate.SetActive(true);
        }
        else
        {
            imageVibrate.SetActive(false);
        }
        isVibrate = !isVibrate;
    }
    public void _OnClickVFXButton()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        if (!isVFX)
        {
            imageVFX.SetActive(true);
        }
        else
        {
            imageVFX.SetActive(false);
        }
        isVFX = !isVFX;
    }

    public void _OnClickZoomButton()
    {
        isZoomFullScreen = true;
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        if (!isZoom)
        {
            imageZoomOut.SetActive(true);
            imageZoomIn.SetActive(false);
            hideUI.DOAnchorPosY(-540, 0.5f).SetEase(Ease.InBack);
            zoomUI.DOAnchorPosX(66, 0.5f).SetEase(Ease.InBack);
        }
        else
        {
            imageZoomOut.SetActive(false);
            imageZoomIn.SetActive(true);
            hideUI.DOAnchorPosY(-35, 0.5f).SetEase(Ease.OutBack);
            zoomUI.DOAnchorPosX(-66, 0.5f).SetEase(Ease.OutBack);
        }
        isZoom = !isZoom;
    }

    #endregion
}
