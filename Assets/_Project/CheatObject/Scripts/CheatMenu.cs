using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Cheat
{
    public class CheatMenu : MonoBehaviour
    {
        [SerializeField] RectTransform mainPanel;
        [SerializeField] List<CheatPanelElement> listCheatPanel;
        [SerializeField] CheatOpener cheatOpener;
        bool isCheck1;
        bool isCheck2;
        void Start()
        {
            listCheatPanel.ForEach((panel) => panel.OnInitialize());
        }
        private void Awake()
        {
            cheatOpener.onOpen += OpenCheat;
        }
        float lastClickCheck1 = 0;
        float clickCheck1Count = 0;
        public void OnClickCheck1()
        {
            float currentTime = Time.timeSinceLevelLoad;
            float durationAmount = (currentTime - lastClickCheck1);

            if (durationAmount < 1f)
            {
                lastClickCheck1 = currentTime;
                clickCheck1Count++;
                if (clickCheck1Count >= 3)
                {
                    SetCheck1();
                }
            }
            else
            {
                lastClickCheck1 = currentTime;
                clickCheck1Count = 0;
            }
        }
        float lastClickCheck2 = 0;
        float clickCheck2Count = 0;
        public void OnClickCheck2()
        {
            float currentTime = Time.timeSinceLevelLoad;
            float durationAmount = (currentTime - lastClickCheck2);

            if (durationAmount < 1f)
            {
                lastClickCheck2 = currentTime;
                clickCheck2Count++;
                if (clickCheck2Count >= 3)
                {
                    SetCheck2();
                }
            }
            else
            {
                lastClickCheck2 = currentTime;
                clickCheck2Count = 0;
            }
        }
        public void SetCheck1()
        {
            isCheck1 = true;
            CheckAndOpen();
        }
        public void SetCheck2()
        {
            isCheck2 = true;
            CheckAndOpen();
        }
        void CheckAndOpen()
        {
            if (isCheck1 && isCheck2)
            {
                isCheck1 = false;
                isCheck2 = false;
                OpenCheat();
            }
        }
        public void OpenCheat()
        {
            mainPanel.gameObject.SetActive(true);
        }
        public void CloseCheat()
        {
            mainPanel.gameObject.SetActive(false);
        }
    }
}
