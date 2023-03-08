using UnityEngine;

namespace _Game.Code.UI
{
    public class EndScreen : MonoBehaviour
    {
        public GameObject content;
        public GameObject win;
        public GameObject fail;
        private void OnEnable()
        {
            GameController.Instance.onFinishGame += ShowMenu;
            content.SetActive(false);
            win.SetActive(false);
            fail.SetActive(false);
        }


        private void ShowMenu(bool state)
        {
            content.SetActive(true);
            win.SetActive(state);
            fail.SetActive(!state);
        }
    }
}