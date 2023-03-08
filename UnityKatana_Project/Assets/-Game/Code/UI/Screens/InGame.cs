using UnityEngine;

namespace _Game.Code.UI
{
    public class InGame : MonoBehaviour
    {
        public GameObject content;
        private void OnEnable()
        {
            content.SetActive(false);
            GameController.Instance.onStartGame += ShowMenu;
            GameController.Instance.onFinishGame += HideMenu;
        }

        private void HideMenu(bool state)
        {
         //   content.SetActive(false);
        }

        private void ShowMenu()
        {
            content.SetActive(true);
        }
    }
}