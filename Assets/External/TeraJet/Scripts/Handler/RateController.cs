using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeraJet
{
    public class RateController : MonoBehaviour
    {

        [SerializeField] List<RateItemHandler> m_RateItems;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < m_RateItems.Count; i++)
            {
                m_RateItems[i].m_Index = i;
                m_RateItems[i].OnRateClick += OnRateItemClick;
            }
        }

        public void OnRateItemClick(int index)
        {
            Debug.Log("Click on: " + index);
            if (index >= m_RateItems.Count || index < 0) return;
            if (m_RateItems[index].m_IsEnable)
            {
                for (int i = index + 1; i < m_RateItems.Count; i++)
                {
                    m_RateItems[i].SetEnable(false);
                }
            }
            try
            {
                for (int j = 0; j <= index; j++)
                {
                    m_RateItems[j].SetEnable(true);
                }
            }
            catch (Exception e)
            {
                Debug.Log("error: " + e.Message);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < m_RateItems.Count; i++)
            {
                m_RateItems[i].OnRateClick -= OnRateItemClick;
            }
        }

        public void Rate()
        {
            int count = 0;
            foreach (RateItemHandler item in m_RateItems)
            {
                if (item.m_IsEnable)
                {
                    count++;
                }
            }
            if (count >= PlayerPrefsConfig.MINIMUM_RATE_SUBMIT)
            {
                GameUtils.OpenAppStoreGame(Application.identifier);
            }
            else
            {
                // Show Thanks Dialog
                GameUtils.ShowDialog(PopupController.PopupType.SIMPLE);
            }
            Destroy(transform.root.gameObject);
        }
    }
}
