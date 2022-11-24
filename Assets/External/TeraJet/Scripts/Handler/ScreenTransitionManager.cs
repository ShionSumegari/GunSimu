using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace TeraJet
{
    public class ScreenTransitionManager : Singleton<ScreenTransitionManager>
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] RectTransform currentMask;
        public float maxDeltaSize = 1300;
        public float loadSceneAnimDuration = 1f;
        public float endSceneAnimDuration = 0.5f;

        private Tween fadeTween;

        public event System.Action OnOpenTransitionAnimationCompleted;
        public event System.Action OnCloseTransitionAnimationCompleted;

        public delegate void OnOpenCompleted();
        public delegate void OnCloseCompleted();
        public delegate void OnHighLightCompleted();


        public Vector3 currentHighLightSize;
        // Update is called once per frame

        public void StartScene(OnOpenCompleted logEventOK)
        {
            currentMask.DOSizeDelta(new Vector3(maxDeltaSize, maxDeltaSize, maxDeltaSize), loadSceneAnimDuration).Play().OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                logEventOK();
            });
        }

        public void EndScene(OnCloseCompleted logEventOK)
        {
            currentMask.DOSizeDelta(Vector3.zero, endSceneAnimDuration).Play().OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                if (OnCloseTransitionAnimationCompleted != null)
                {
                    OnCloseTransitionAnimationCompleted();
                }
                logEventOK();
            });
        }

        public void HighLightArea(OnHighLightCompleted onHighLightCompleted)
        {
            currentMask.DOSizeDelta(currentHighLightSize, endSceneAnimDuration).Play().OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                if (OnCloseTransitionAnimationCompleted != null)
                {
                    OnCloseTransitionAnimationCompleted();
                }
                onHighLightCompleted();
            });
        }

        void FadeIn(float duration)
        {
            Fade(1f, duration, () =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }

        void FadeOut(float duration)
        {
            Fade(0f, duration, () =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
        }

        void Fade(float endValue, float duration, TweenCallback onEnd)
        {
            if (fadeTween != null)
            {
                fadeTween.Kill();
            }
            fadeTween = canvasGroup.DOFade(endValue, duration);
            fadeTween.onComplete = onEnd;
        }
    }
}


