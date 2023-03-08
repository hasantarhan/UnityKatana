using System;
using UnityEngine;

namespace _Game.Code.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject content;
        private void OnEnable()
        {
            content.SetActive(false);
            GameController.Instance.onBootGameComplete += ShowMenu;
            GameController.Instance.onStartGame += HideMenu;
  
        }

        private void StartGame()
        {
            GameController.Instance.StartGame();
        }
        
        private void HideMenu()
        {
            content.SetActive(false);
        }

        private void ShowMenu()
        {
            content.SetActive(true);
        }
    }
}