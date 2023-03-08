using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Code.UI
{
    public class AnimatedButton : Button
    {
        private bool isClicked;

        protected override void Awake()
        {
            transition = Transition.None;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!isClicked)
            {
                isClicked = true;
                base.OnPointerClick(eventData);

                DOTween.Sequence().AppendInterval(0.3f).AppendCallback(() => isClicked = false);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(0.9f, 0.2f).onComplete += () => { base.OnPointerDown(eventData); };
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (gameObject)
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack, 4, 0f).onComplete +=
                    () => { base.OnPointerUp(eventData); };
        }
    }
}