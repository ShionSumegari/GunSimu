using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogleGroupController : MonoBehaviour
{
    [SerializeField] public Toggle singleShot;
    [SerializeField] Toggle burstMode;
    [SerializeField] Toggle auto;
    [SerializeField] Toggle shake;

    [SerializeField] public Sprite checkBox;
    [SerializeField] Sprite unCheckBox;

    private void Start()
    {
        ResetToggle();
        singleShot.isOn = true;
        singleShot.targetGraphic.GetComponent<Image>().sprite = checkBox;
        GamePlayController.Instance.isSingleShot = true;
    }
    public void ResetToggle()
    {
        singleShot.isOn = false;
        GamePlayController.Instance.isSingleShot = false;
        singleShot.targetGraphic.GetComponent<Image>().sprite = unCheckBox;

        burstMode.isOn = false;
        GamePlayController.Instance.isBurtMode = false;
        burstMode.targetGraphic.GetComponent<Image>().sprite = unCheckBox;

        auto.isOn = false;
        GamePlayController.Instance.isAuto = false;
        auto.targetGraphic.GetComponent<Image>().sprite = unCheckBox;

        shake.isOn = false;
        GamePlayController.Instance.isShake = false;
        shake.targetGraphic.GetComponent<Image>().sprite = unCheckBox;

    }
    public void _OnClickToggleSingleShot()
    {
        GameManager.Instance.audioManager.PlaySFX("Switch", 1f);
        ResetToggle();
        singleShot.isOn = true;
        singleShot.targetGraphic.GetComponent<Image>().sprite = checkBox;
        GamePlayController.Instance.isSingleShot = true;
    }   
    
    public void _OnClickToggleBurstMode()
    {
        GameManager.Instance.audioManager.PlaySFX("Switch", 1f);
        ResetToggle();
        burstMode.isOn = true;
        burstMode.targetGraphic.GetComponent<Image>().sprite = checkBox;
        GamePlayController.Instance.isBurtMode = true;
    }

    public void _OnClickToggleAuto()
    {
        GameManager.Instance.audioManager.PlaySFX("Switch", 1f);
        ResetToggle();
        auto.isOn = true;
        auto.targetGraphic.GetComponent<Image>().sprite = checkBox;
        GamePlayController.Instance.isAuto = true;
    }

    public void _OnClickToggleShake()
    {
        GameManager.Instance.audioManager.PlaySFX("Switch", 1f);
        ResetToggle();
        shake.isOn = true;
        shake.targetGraphic.GetComponent<Image>().sprite = checkBox;
        GamePlayController.Instance.isShake = true;
    }
}
