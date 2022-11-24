using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderBannerController : MonoBehaviour
{
    [SerializeField] Transform slider;
    [SerializeField] GameObject container;
    [SerializeField] GameObject gunHolder;

    [SerializeField] string currentGunType;
    bool isClickedSlider;
    private void Start()
    {
        slider.GetComponent<RectTransform>().anchoredPosition = transform.GetChild(0).GetComponent<GunHolder>().sliderPos;
        float height = container.GetComponent<RectTransform>().rect.height;
        container.GetComponent<GridLayoutGroup>().cellSize = new 
            Vector2(container.GetComponent<GridLayoutGroup>().cellSize.x, height);
        slider.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<GridLayoutGroup>().cellSize.x + 30,
            slider.GetComponent<RectTransform>().sizeDelta.y);
    }
    public void SetupSlider(GunHolder.GunType guntype)
    {
        foreach(Transform gunHolder in transform)
        {
            if(gunHolder.GetComponent<GunHolder>().gunType == guntype && !isClickedSlider)
            {
                slider.GetComponent<RectTransform>().DOAnchorPos(gunHolder.GetComponent<GunHolder>().sliderPos, 0.2f).SetEase(Ease.InOutBack)
                    .OnComplete(()=> { 
                        currentGunType = guntype.ToString();
                    });
            }
        }
    }

    public void _OnClickGunButton(Button targetButton)
    {
        if (targetButton.GetComponent<GunHolder>().gunType.ToString() != currentGunType)
        {
            isClickedSlider = true;
            slider.GetComponent<RectTransform>().anchoredPosition = targetButton.GetComponent<GunHolder>().sliderPos;
            currentGunType = targetButton.GetComponent<GunHolder>().gunType.ToString();
            gunHolder.GetComponent<RectTransform>().DOAnchorPosX(-targetButton.GetComponent<GunHolder>().gunPos, 1f).SetEase(Ease.OutCubic)
                .OnComplete(()=> {
                    isClickedSlider = false;
                });
        } 
    }
}
