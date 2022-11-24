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
    [SerializeField] Transform lightning;
    [SerializeField] RectTransform bullet;
    [SerializeField] Text gunNameText;
    [SerializeField] public Text bulletText;
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
        float delayTime, float burstDelay, string gunName, Gun.TypeGun typeGun, float timeReload)
    {
        gunPannel.SetActive(true);
        OnSetupGunPannel(targetImg, targetLight, targetBulletAmount, targetBulletImg,
            decoilBack, decoilUp, decoilBackValue, decoilUpValue, gunLightning, burstMode, delayTime,
            burstDelay, gunName, typeGun,timeReload);
    }

    public void _OnCloseGunPannel()
    {
        gunPannel.SetActive(false);
        GameManager.Instance.audioManager.PlaySFX("UiClick", 1f);
        GameManager.Instance.audioManager.StopSFX(gunHolder.GetComponent<GunDecoil>().gunName.text + "R", 1f);
        GameManager.Instance.audioManager.StopSFX(gunHolder.GetComponent<GunDecoil>().gunName.text , 1f);
        GamePlayController.Instance.SwipeClose();
    }

    public void OnSetupGunPannel(Image targetImg, RectTransform targetLight, int targetBulletAmount , Sprite targetBulletImg,
        bool decoilBack, bool decoilUp, float decoilBackValue, float decoilUpValue, Sprite gunLightning, int burstModeGun, 
        float delayTime,float holdDelay, string gunName, Gun.TypeGun typeGun, float timeReload)
    {
        GamePlayController.Instance.reloadTime.SetActive(false);
        GamePlayController.Instance.gunImage.color = new Color32(255, 255, 255, 255);
        gunHolder.GetComponent<GunDecoil>().smoke.Stop();
        gunHolder.GetComponent<GunDecoil>().smoke.Clear();
        gunImage.sprite = targetImg.sprite;
        gunImageDecoil.sprite = targetImg.sprite;
        lightGun.anchorMin = targetLight.anchorMin;
        lightGun.anchorMax = targetLight.anchorMax;
        bullet.anchorMin = targetLight.anchorMin;
        bullet.anchorMax = targetLight.anchorMax;
        gunHolder.GetComponent<GunDecoil>().decoidBack = decoilBack;
        gunHolder.GetComponent<GunDecoil>().decoilUp = decoilUp;
        gunHolder.GetComponent<GunDecoil>().backDecoilValue = decoilBackValue;
        gunHolder.GetComponent<GunDecoil>().backValue = decoilBackValue;
        gunHolder.GetComponent<GunDecoil>().upDecoilValue = decoilUpValue;
        lightning.GetComponent<Image>().sprite = gunLightning;
        reloadTime = timeReload;
        typeGunImg = typeGun.ToString();
        gunNameText.text = gunName;
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
    }
    #endregion
}
