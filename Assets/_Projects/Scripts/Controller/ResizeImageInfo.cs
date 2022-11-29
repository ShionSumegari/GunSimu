using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeImageInfo : MonoBehaviour
{
    [SerializeField] Image resizeImage;
    [SerializeField] GridLayoutGroup holder;

    private void Update()
    {
        ResizeGun();
    }
    public void ResizeGun()
    {
        float width = resizeImage.sprite.rect.width;
        float height = resizeImage.sprite.rect.height;

        float realRatio = width / height;
        float holderRatio = holder.GetComponent<RectTransform>().rect.width / holder.GetComponent<RectTransform>().rect.height;

        if (realRatio >= holderRatio)
        {
            float resizeWidth = holder.GetComponent<RectTransform>().rect.width;
            float resizeHeight = height * resizeWidth / width;

            holder.cellSize = new Vector2(resizeWidth, resizeHeight);
        }
        else
        {
            float resizeHeight = holder.GetComponent<RectTransform>().rect.height;

            float resizeWidth = width * resizeHeight / height;

            holder.cellSize = new Vector2(resizeWidth, resizeHeight);
        }
    }
}
