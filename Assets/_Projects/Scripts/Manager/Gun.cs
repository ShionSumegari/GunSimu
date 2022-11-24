using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public RectTransform lightningRect;
    public enum TypeGun
    {
         PISTOLS,
         SHOTGUNS,
         ASSAULTRIFLES,
         SMGS,
         MACHINEGUNS,
         SNIPERRIFLES
    }
    public TypeGun typeGun;

    public int gunId;

    public Image gunImage;

    public string gunName;

    public Sprite bulletImage;

    public int bulletAmount;

    public Sprite gunLightning;

    public int burstMode;

    public bool decoidBack;
    public bool decoilUp;

    public float backDecoilValue;
    public float upDecoilValue;

    public float burstDelay;
    public float holdDelay;

    public float timeReload;


    public void _OnClickGunButton(Button targetButton)
    {
        GameplayUIController.Instance.OnOpenGunPannel(targetButton.GetComponent<Gun>().gunImage,
            targetButton.GetComponent<Gun>().lightningRect, targetButton.GetComponent<Gun>().bulletAmount,
            targetButton.GetComponent<Gun>().bulletImage, targetButton.GetComponent<Gun>().decoidBack,
            targetButton.GetComponent<Gun>().decoilUp, targetButton.GetComponent<Gun>().backDecoilValue,
            targetButton.GetComponent<Gun>().upDecoilValue, targetButton.GetComponent<Gun>().gunLightning,
            targetButton.GetComponent<Gun>().burstMode, targetButton.GetComponent<Gun>().burstDelay,
            targetButton.GetComponent<Gun>().holdDelay, targetButton.GetComponent<Gun>().gunName,
            targetButton.GetComponent<Gun>().typeGun, targetButton.GetComponent<Gun>().timeReload);
    }

    public void GunInformation()
    {

    }
}
