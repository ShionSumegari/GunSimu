using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwipeToReloadController : MonoBehaviour
{
    [SerializeField] RectTransform swipeHand;
    [SerializeField] RectTransform reloadImage;

    private void Start()
    {
        swipeHand.DOAnchorPosX(686, 1.5f).SetEase(Ease.OutQuart).SetLoops(-1, LoopType.Restart);
        reloadImage.DOLocalRotate(new Vector3(0, 0, -180), 1.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Restart);
    }
}
