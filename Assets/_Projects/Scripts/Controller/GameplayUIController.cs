using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayUIController : MonoBehaviour
{
   
    public string typeGunImg;

    [Header("Home Pannel: ")]
    [SerializeField] Button buttonBack; 
    [SerializeField] Button buttonShop;
    [SerializeField] Button buttonLoveGun;
    [SerializeField] GameObject exitPannel;
    [SerializeField] CanvasGroup pannel;

    [Header("Gun Pannel: ")]
    [SerializeField] GameObject gunPannel;
    [SerializeField] ResizeImage resizeGun;
    [SerializeField] Image gunImage;
    [SerializeField] Image gunImageDecoil;
    [SerializeField] public Transform bulletHolder;
    [SerializeField] public Transform overBulletHolder;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform lightGun;
    [SerializeField] Transform gunHolder;
    [SerializeField] RectTransform bullet;
    [SerializeField] Text gunNameText;
    [SerializeField] Text gunNameTextInf;
    [SerializeField] public Text bulletText;
    [SerializeField] RectTransform LightGowithgun;
    [SerializeField] TogleGroupController togle;

    [Header("Gun Infor:")]
    [SerializeField] Image imgGunInfo;
    [SerializeField] GameObject infoPannel;
    [SerializeField] CanvasGroup pannelIn;
    [SerializeField] Text m_Type;
    [SerializeField] Text m_Place;
    [SerializeField] Text m_InSer;
    [SerializeField] Text m_Desinger;
    [SerializeField] Text m_Designed;
    [SerializeField] Text m_Cartri;
    [SerializeField] Text m_NameGun;
    [SerializeField] Text m_Manazine;
    public int burstMode;
    public float reloadTime;


    public Sprite targetImgBullet;
    #region SingleTon
    public static GameplayUIController Instance;

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

    #region GUN_PANNEL

    public void OnOpenGunPannel(Image targetImg, RectTransform targetLight, int targetBulletAmount, Sprite targetBulletImg,
        bool decoilBack, bool decoilUp, float decoilBackValue, float decoilUpValue, Sprite gunLightning,int burstMode ,
        float delayTime, float burstDelay, string gunName, Gun.TypeGun typeGun, float timeReload, bool isGoWithGun)
    {
        gunPannel.SetActive(true);
        OnSetupGunPannel(targetImg, targetLight, targetBulletAmount, targetBulletImg,
            decoilBack, decoilUp, decoilBackValue, decoilUpValue, gunLightning, burstMode, delayTime,
            burstDelay, gunName, typeGun,timeReload,isGoWithGun);
    }

    public void OnSetUpInfor(string type, string placeOfOr, string inSer, string designer, string designed, string cartrig,
        string name, string manazine)
    {
        m_Type.text = "Type: " + type;
        m_Place.text = "Place of origin: " + placeOfOr;
        m_InSer.text = "In Service: " + inSer;
        m_Desinger.text = "Designer: " + designer;
        m_Designed.text = "Designed: " + designed;
        m_Cartri.text = "Cartridge: " + cartrig;
        m_NameGun.text = "Name: " + name;
        m_Manazine.text = "Magazine capacity: " + manazine;
    }

    public void _OnCloseGunPannel()
    {
        togle.ResetToggle();
        togle.singleShot.isOn = true;
        togle.singleShot.targetGraphic.GetComponent<Image>().sprite = togle.checkBox;
        GamePlayController.Instance.isSingleShot = true;
        gunPannel.SetActive(false);
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        GameManager.Instance.audioManager.StopSFX(gunHolder.GetComponent<GunDecoil>().gunName.text + "R", 1f);
        GameManager.Instance.audioManager.StopSFX(gunHolder.GetComponent<GunDecoil>().gunName.text , 1f);
        GamePlayController.Instance.SwipeClose();
    }

    public void OnSetupGunPannel(Image targetImg, RectTransform targetLight, int targetBulletAmount , Sprite targetBulletImg,
        bool decoilBack, bool decoilUp, float decoilBackValue, float decoilUpValue, Sprite gunLightning, int burstModeGun, 
        float delayTime,float holdDelay, string gunName, Gun.TypeGun typeGun, float timeReload, bool isGoWithGun)
    {

        GamePlayController.Instance.reloadTime.SetActive(false);
        GamePlayController.Instance.gunImage.color = new Color32(255, 255, 255, 255);
        gunHolder.GetComponent<GunDecoil>().smoke.Stop();
        gunHolder.GetComponent<GunDecoil>().smoke.Clear();
        gunImage.sprite = targetImg.sprite;
        imgGunInfo.sprite = targetImg.sprite;
        gunImageDecoil.sprite = targetImg.sprite;
        lightGun.anchorMin = targetLight.anchorMin;
        lightGun.anchorMax = targetLight.anchorMax;
        LightGowithgun.anchorMin = targetLight.anchorMin;
        LightGowithgun.anchorMax = targetLight.anchorMax;
        bullet.anchorMin = targetLight.anchorMin;
        bullet.anchorMax = targetLight.anchorMax;
        gunHolder.GetComponent<GunDecoil>().decoidBack = decoilBack;
        gunHolder.GetComponent<GunDecoil>().decoilUp = decoilUp;
        gunHolder.GetComponent<GunDecoil>().backDecoilValue = decoilBackValue;
        gunHolder.GetComponent<GunDecoil>().backValue = decoilBackValue;
        gunHolder.GetComponent<GunDecoil>().upDecoilValue = decoilUpValue;
        lightGun.GetComponent<Image>().sprite = gunLightning;
        LightGowithgun.GetComponent<Image>().sprite = gunLightning;
        reloadTime = timeReload;
        typeGunImg = typeGun.ToString();
        gunNameText.text = gunName;
        gunNameTextInf.text = gunName;
        GamePlayController.Instance.delayShot = delayTime;
        GamePlayController.Instance.delayHold = holdDelay;
        burstMode = burstModeGun;
        targetImgBullet = targetBulletImg;
        foreach(Transform bullet in bullet.transform)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.GetComponent<Image>().sprite = targetBulletImg;
            }
        }
        GamePlayController.Instance.bulletamount = targetBulletAmount;
        // Set Up Bullet
        foreach (Transform bullet in bulletHolder)
        {
            Destroy(bullet.gameObject);
        }
        for (int i = 0; i < targetBulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletHolder);
            bullet.GetComponent<Image>().sprite = targetBulletImg;
        }
        overBulletHolder.GetChild(0).GetComponent<Image>().sprite = targetBulletImg;
        bulletText.text = "X " + targetBulletAmount;

        if(targetBulletAmount > 50)
        {
            bulletHolder.gameObject.SetActive(false);
            overBulletHolder.gameObject.SetActive(true);
        }
        else
        {
            bulletHolder.gameObject.SetActive(true);
            overBulletHolder.gameObject.SetActive(false);
        }
        if (isGoWithGun)
        {
            LightGowithgun.gameObject.SetActive(true);
            lightGun.gameObject.SetActive(false);
        }
        else
        {
            LightGowithgun.gameObject.SetActive(false);
            lightGun.gameObject.SetActive(true);
        }
    }
    #endregion

    #region HOME_PANEL

    public void _OnclickExitBtn()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        pannel.gameObject.SetActive(true);
        pannel.DOFade(1, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            exitPannel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        });
    }
    public void _OnClickYesExit()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        Application.Quit();
    }

    public void _OnClickNoExit()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        exitPannel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(()=> {
            pannel.DOFade(0, 0.1f).SetEase(Ease.Linear).OnComplete(()=> { 
                pannel.gameObject.SetActive(false);
            });
        });
    }

    public void _OnClickInfoBtn()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        pannelIn.gameObject.SetActive(true);
        pannelIn.DOFade(1, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            infoPannel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack);
        });
    }
    public void _OncloseInfo()
    {
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        infoPannel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(()=> {
            pannelIn.DOFade(0, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                pannelIn.gameObject.SetActive(false);
            });
        });
    }
    #endregion
}
