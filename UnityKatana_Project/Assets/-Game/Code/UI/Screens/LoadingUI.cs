using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    public class LoadingUI : MonoBehaviour
    {
        public CanvasGroup content;
        public TextMeshProUGUI loadingText;

        private void Awake()
        {
            content.gameObject.SetActive(true);
            content.alpha = 1;
        }

        private void Start()
        {
            DOTween.Sequence().AppendInterval(0.25f).AppendCallback(delegate
            {
                if (gameObject.activeInHierarchy)
                    loadingText.gameObject.SetActive(true);
            });
        }

        private void OnEnable()
        {
            GameController.Instance.onBootGameComplete += BootGameCompleted;
        }

        private void BootGameCompleted()
        {
            content.DOFade(0, 0.1f).onComplete += () =>
            {
                content.gameObject.SetActive(false);
            };
        }
    }
}