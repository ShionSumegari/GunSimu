using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AndroidNativeCore;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;

public class GunDecoil : MonoBehaviour
{
    [SerializeField] RectTransform gunHolder;
    [SerializeField] RectTransform gunImage;
    [SerializeField] Transform lightning;
    [SerializeField] Transform lightningWithGun;
    [SerializeField] Transform bulletholder;
    [SerializeField] CanvasGroup splashImage;
    [SerializeField] GameObject splashPlugin;
    [SerializeField] public ParticleSystem smoke;
    public Text gunName;


    public bool decoidBack;
    public bool decoilUp;

    public float backDecoilValue;
    public float upDecoilValue;

    public float backValue;

    private float gunDecoilTime;
    private bool isStoping;

    Coroutine coroutine;

    private void Start()
    {
        smoke.Stop();
        smoke.Clear();
    }
    public void DeCoilGun()
    {
        gunDecoilTime = GamePlayController.Instance.delayShot;
        backDecoilValue = backValue;
        if(GamePlayController.Instance.isRotate)
        {
            backDecoilValue = -backValue;
        }
        if (!GamePlayController.Instance.isFlashLight)
        {
            splashPlugin.GetComponent<FlashlightPlugin>().TurnOn();
        }
        if (!GamePlayController.Instance.isVibrate)
        {
            MMVibrationManager.TransientHaptic(0.5f, 1);
            Debug.Log("Vibrate");
        }
        GameManager.Instance.audioManager.PlaySFX(gunName.text, 1f);
        splashImage.DOFade(1, 0.05f).SetEase(Ease.Linear).OnComplete(() =>
        {
            splashImage.DOFade(0, 0.05f).SetEase(Ease.Linear);
            splashPlugin.GetComponent<FlashlightPlugin>().TurnOff();
        });
        if (!GamePlayController.Instance.isVFX)
        {

            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(SmokeTime());
            lightningWithGun.GetComponent<RectTransform>().DOScale(Vector3.one * 2, 0.05f).SetEase(Ease.Linear);
            lightning.GetComponent<RectTransform>().DOScale(Vector3.one * 2, 0.05f).SetEase(Ease.Linear)
                .OnComplete(() => {
                    lightning.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.05f).SetEase(Ease.Linear);
                    lightningWithGun.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.05f).SetEase(Ease.Linear);
                });
            float speed = 0.05f;
            if(GameplayUIController.Instance.typeGunImg.CompareTo(Gun.TypeGun.ROCKET.ToString()) == 0)
            {
                speed = 0.5f;
            }
            foreach (Transform bullet in bulletholder)
            {
                if (!bullet.gameObject.activeSelf)
                {
                    bullet.GetComponent<Image>().SetNativeSize();
                    bullet.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 0);
                    bullet.gameObject.SetActive(true);
                    bullet.GetComponent<RectTransform>().DOAnchorPosX(-1000, speed).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        bullet.gameObject.SetActive(false);
                    });
                    break;
                }
            }
        }
        if (decoidBack)
        {
            gunImage.DOAnchorPosX(backDecoilValue, 0.05f).SetEase(Ease.Linear).OnComplete(()=> {
                gunImage.DOAnchorPosX(0f, 0.05f).SetEase(Ease.Linear);
            });
        }
        if (decoilUp)
        {
            gunImage.DOLocalRotate(new Vector3(gunImage.localEulerAngles.x, gunImage.localEulerAngles.y, upDecoilValue), 0.05f).SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     gunImage.DOLocalRotate(new Vector3(gunImage.localEulerAngles.x, gunImage.localEulerAngles.y, 0f), 0.05f).SetEase(Ease.Linear);
                 });
        }
    }
    IEnumerator SmokeTime()
    {
        if (!smoke.isStopped && isStoping)
        {
            smoke.Clear();
        }
        if (!smoke.isPlaying)
        {
            isStoping = false;
            smoke.Play();
        }
        yield return new WaitForSeconds(2f);
        isStoping = true;
        smoke.Stop();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            splashPlugin.GetComponent<FlashlightPlugin>().TurnOff();
        }
    }
    private void OnApplicationQuit()
    {
        splashPlugin.GetComponent<FlashlightPlugin>().TurnOff();
    }
}
