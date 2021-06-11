using Cysharp.Threading.Tasks;
using DG.Tweening;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace pe9.Fifteen.UI
{
    public class UIHub : MonoBehaviour
    {
        private const float FadeDuration = 1.0f;

        [SerializeField]
        private CanvasGroup Background;

        [SerializeField]
        private StartPopup StartPopup;

        [SerializeField]
        private WinPopup WinPopup;

        [SerializeField]
        private GameplayUI GameplayUI;


        private void Start()
        {
            HideAll();
        }

        private void HideAll()
        {
            Background.alpha = 0;
            WinPopup.gameObject.SetActive(false);
            StartPopup.gameObject.SetActive(false);
            GameplayUI.SetActive(false);
        }


        public async UniTask<GameSetup> GetGameSetup()
        {
            var setup = await StartPopup.WaitForStartSubmit();
            return setup;
        }

        public async UniTask HideStartGamePopup()
        {
            await TaskHelpers.WaitFor(Background.DOFade(0, FadeDuration));
            StartPopup.gameObject.SetActive(false);
        }

        public async UniTask ShowStartGamePopup()
        {
            StartPopup.gameObject.SetActive(true);
            await TaskHelpers.WaitFor(Background.DOFade(1, FadeDuration));
        }
    }
}
