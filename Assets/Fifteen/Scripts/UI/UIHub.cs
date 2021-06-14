using Cysharp.Threading.Tasks;
using DG.Tweening;
using pe9.Fifteen.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace pe9.Fifteen.UI
{
    public class UIHub : MonoBehaviour
    {
        private enum UIState
        {
            WithoutUI,
            SetupRequest,
            Gameplay,
            Win,
        }

        private const float FadeDuration = 1.0f;

        [SerializeField]
        private CanvasGroup Background;

        [SerializeField]
        private StartPopup StartPopup;

        [SerializeField]
        private ConfirmationPopup WinPopup;

        [SerializeField]
        private GameplayUI GameplayUI;


        private UIState State;

        public async UniTask<GameSetup> SetupRequest()
        {
            await TransitionTo(UIState.SetupRequest);
            return await StartPopup.WaitForStartSubmit();
        }

        public async UniTask Gameplay()
        {
            await TransitionTo(UIState.Gameplay);
        }

        public async UniTask Win()
        {
            await TransitionTo(UIState.Win);
        }


        private void Start()
        {
            HideAll();
            State = UIState.WithoutUI;
        }
        private void HideAll()
        {
            Background.alpha = 0;
            WinPopup.gameObject.SetActive(false);
            StartPopup.gameObject.SetActive(false);
            GameplayUI.SetActive(false);
        }

        private async UniTask TransitionTo(UIState state)
        {
            switch (state)
            {
                case UIState.SetupRequest:
                    await ShowPopup(StartPopup.gameObject);
                    break;
                case UIState.Gameplay:
                    await HidePopup(StartPopup.gameObject);
                    break;
                case UIState.Win:
                    await ShowPopup(WinPopup.gameObject);

                    await WinPopup.WaitForConfirmation();
                    await HidePopup(WinPopup.gameObject);
                    break;
                default:
                    throw new ArgumentException($"Unable to perform transition to state {state}");
            }
        }


        private async UniTask<GameSetup> GetGameSetup()
        {
            var setup = await StartPopup.WaitForStartSubmit();
            return setup;
        }

        private async UniTask HideStartGamePopup()
        {

        }

        private async UniTask ShowStartGamePopup()
        {
            
        }

        private async UniTask ShowWinPopup()
        {

        }

        private async UniTask ShowPopup(GameObject popup)
        {
            popup.gameObject.SetActive(true);
            await TaskHelpers.WaitFor(Background.DOFade(1, FadeDuration));
        }

        private async UniTask HidePopup(GameObject popup)
        {
            await TaskHelpers.WaitFor(Background.DOFade(0, FadeDuration));
            popup.gameObject.SetActive(false);
        }
    }
}
