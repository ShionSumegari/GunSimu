using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Cheat
{
    public class LevelPanel : CheatPanelElement
    {
        public int levelCount;
        [SerializeField] LevelButton _viewPreb;
        [SerializeField] RectTransform _contentHolder;
        public override void OnInitialize()
        {
            for(int i = 0; i < levelCount; i++)
            {
                LevelButton btn = Instantiate(_viewPreb, _contentHolder.transform);
                btn.gameObject.SetActive(true);
                btn.OnInitialize(i);
            }
        }
    }
}
