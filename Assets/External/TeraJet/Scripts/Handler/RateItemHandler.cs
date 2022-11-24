using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeraJet
{
    public class RateItemHandler : MonoBehaviour
    {

        [SerializeField] GameObject m_EnableSprite;
        [SerializeField] GameObject m_DisableSprite;
        public int m_Index;
        public bool m_IsEnable = false;

        public delegate void RateClick(int index);
        public event RateClick OnRateClick;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick()
        {
            if (OnRateClick != null)
            {
                OnRateClick(m_Index);
            }
        }

        public void SetEnable(bool isEnable)
        {
            m_IsEnable = isEnable;
            m_EnableSprite.SetActive(m_IsEnable);
            m_DisableSprite.SetActive(!m_IsEnable);
        }
    }
}
