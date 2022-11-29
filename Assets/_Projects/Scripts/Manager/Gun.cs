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

    public bool isGoWithGun;

    [Header("Infor Gun:")]
    public string type;
    public string placeOfOrigin;
    public string inService;
    public string designer;
    public string designed;
    public string cartridge;
    public string name;
    public string magazineCapacity;


    public void _OnClickGunButton(Button targetButton)
    {
        GameManager.Instance.audioManager.PlaySFX("ClickGun", 1f);
        GameplayUIController.Instance.OnOpenGunPannel(targetButton.GetComponent<Gun>().gunImage,
            targetButton.GetComponent<Gun>().lightningRect, targetButton.GetComponent<Gun>().bulletAmount,
            targetButton.GetComponent<Gun>().bulletImage, targetButton.GetComponent<Gun>().decoidBack,
            targetButton.GetComponent<Gun>().decoilUp, targetButton.GetComponent<Gun>().backDecoilValue,
            targetButton.GetComponent<Gun>().upDecoilValue, targetButton.GetComponent<Gun>().gunLightning,
            targetButton.GetComponent<Gun>().burstMode, targetButton.GetComponent<Gun>().burstDelay,
            targetButton.GetComponent<Gun>().holdDelay, targetButton.GetComponent<Gun>().gunName,
            targetButton.GetComponent<Gun>().typeGun, targetButton.GetComponent<Gun>().timeReload,
            targetButton.GetComponent<Gun>().isGoWithGun);
        GunInformation(targetButton);
    }

    public void GunInformation(Button targetButton)
    {
        GameplayUIController.Instance.OnSetUpInfor(targetButton.GetComponent<Gun>().type, targetButton.GetComponent<Gun>().placeOfOrigin,
            targetButton.GetComponent<Gun>().inService, targetButton.GetComponent<Gun>().designer, targetButton.GetComponent<Gun>().designed,
            targetButton.GetComponent<Gun>().cartridge, targetButton.GetComponent<Gun>().name, targetButton.GetComponent<Gun>().magazineCapacity);
    }
}
