using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TeraJet
{
    public class TerajetManager : Singleton<TerajetManager>
    {
        public bool m_TestMode;

        // Start is called before the first frame update
        void Start()
        {
            TeraJetClient.Initialize();
        }

        private void Update()
        {
            if (m_TestMode)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ShowRatePopup();
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    ShowUpdatePopup();
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    ScreenTransitionManager.Instance.StartScene(() => { });
                }

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    ScreenTransitionManager.Instance.HighLightArea(() => { });
                }
            }

        }

        public void ShowUpdatePopup()
        {
            GameUtils.ShowDialog(PopupController.PopupType.UPDATE);
        }

        public void ShowRatePopup()
        {
            GameUtils.ShowDialog(PopupController.PopupType.RATE);
        }

        public void ShowMessageDialog()
        {
            GameUtils.ShowDialog(PopupController.PopupType.SIMPLE);
        }
    }
}
