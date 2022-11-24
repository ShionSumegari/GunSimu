using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeraJet
{
    public class PopupController : MonoBehaviour
    {

        public enum PopupType
        {
            SIMPLE, LACK_OF_RESOURCE, PROGRESS, CONFIRM_PURCHASED, BONUS, REWARD, RATE, UPDATE, CONNECTION_REQUIRED
        }
        [SerializeField] List<TeraPopup> m_Pops;
        [SerializeField] GameObject m_ButtonClose;

        public PopupType m_Type;

        public bool isGift;
        // Start is called before the first frame update
        void Start()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (TeraPopup pop in m_Pops)
            {
                pop.gameObject.SetActive(pop.type == m_Type);
                if(pop.type == m_Type)
                    m_ButtonClose.SetActive(pop.isCloseable);
            }
        }

        void OnRateButtonClick()
        {

        }

        void OnUpdateButtonClick()
        {

        }

        public void OnClose()
        {
            Destroy(transform.root.gameObject);
        }

        public void CloseApplication()
        {
            Application.Quit();
        }

        public void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void UpdateClick()
        {
            GameUtils.OpenAppStoreGame(Application.identifier);
            CloseApplication();
        }
    }
}
