using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResizeImage : MonoBehaviour
{
    [SerializeField] Image resizeImage;
    [SerializeField] GridLayoutGroup holder;
    [SerializeField] Slider zoomSlider;
    [SerializeField] RectTransform zoomPannel;
    [SerializeField] RectTransform unZoomPannel;
    float height_Scale;
    float height_Scale_Zoom;

    private void Start()
    {
        height_Scale = holder.GetComponent<RectTransform>().rect.height;
    }
    private void Update()
    {
        ResizeGun();
    }
    public void ResizeGun()
    {
        float width = resizeImage.sprite.rect.width;
        float height = resizeImage.sprite.rect.height;

        height_Scale_Zoom = Mathf.Lerp(holder.GetComponent<RectTransform>().rect.height - 100,
        holder.GetComponent<RectTransform>().rect.height, zoomSlider.value);
        if (!GamePlayController.Instance.isZoom)
        {
            transform.GetComponent<RectTransform>().DOAnchorPos(unZoomPannel.anchoredPosition, 0.2f).SetEase(Ease.Linear);
            if (height_Scale > height_Scale_Zoom && GamePlayController.Instance.isZoomFullScreen)
            {
                height_Scale -= 5;
            }
            else
            {
                GamePlayController.Instance.isZoomFullScreen = false;
                holder.childAlignment = TextAnchor.MiddleCenter;
                height_Scale = Mathf.Lerp(holder.GetComponent<RectTransform>().rect.height - 100,
                holder.GetComponent<RectTransform>().rect.height, zoomSlider.value);
            }
        }
        else if(GamePlayController.Instance.isZoom)
        {
            holder.childAlignment = TextAnchor.MiddleCenter;
            transform.GetComponent<RectTransform>().DOAnchorPos(zoomPannel.anchoredPosition, 0.2f).SetEase(Ease.Linear);
            if (height_Scale < 600)
            {
                height_Scale += 5;
            }
            else
            {
                height_Scale = 600;
            }
        }


        float realRatio = width / height;
        float holderRatio = holder.GetComponent<RectTransform>().rect.width / height_Scale;

        if(realRatio >= holderRatio)
        {
            float resizeWidth = holder.GetComponent<RectTransform>().rect.width;
            float resizeHeight = height * resizeWidth / width;

            holder.cellSize = new Vector2(resizeWidth, resizeHeight);
        }
        else
        {
            float resizeHeight = height_Scale;
            
            float resizeWidth = width * resizeHeight / height;

            holder.cellSize = new Vector2(resizeWidth, resizeHeight);
        }
    }
}
