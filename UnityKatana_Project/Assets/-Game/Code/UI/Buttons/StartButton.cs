using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class StartButton : MonoBehaviour
    {
        private Button button;
        public void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            GameController.Instance.StartGame();
        }
    }
}