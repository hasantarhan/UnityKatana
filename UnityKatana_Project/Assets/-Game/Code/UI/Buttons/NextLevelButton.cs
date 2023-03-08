using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class NextLevelButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Click);
        }

        private void Click()
        {
            GameController.Instance.NextLevel();
        }
    }
}