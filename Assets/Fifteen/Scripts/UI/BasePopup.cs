using Cysharp.Threading.Tasks;
using DG.Tweening;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePopup : MonoBehaviour
    {
        private CanvasGroup CanvasGroup;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void HideImmediate()
        {
            CanvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        public async UniTask Show(float duration)
        {
            gameObject.SetActive(true);
            await TaskHelpers.WaitFor(CanvasGroup.DOFade(1, duration));
        }

        public async UniTask Hide(float duration)
        {
            await TaskHelpers.WaitFor(CanvasGroup.DOFade(0, duration));
            gameObject.SetActive(false);
        }
    }
}
