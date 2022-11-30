using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Cheat
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI levelText;

        int _levelIndex = 0;

        public void OnInitialize(int levelIndex)
        {
            button.onClick.AddListener(OnBtnClick);
            levelText.text = levelIndex.ToString();

            _levelIndex = levelIndex;
        }
        public void OnBtnClick()
        {
            int level = _levelIndex;
            //Write Code here
        }
    }
}
