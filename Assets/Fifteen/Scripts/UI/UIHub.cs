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

        public Action GameAbortRequested;

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
            GameplayUI.RestartButtonClicked += () => GameAbortRequested?.Invoke();

            HideAll();
            State = UIState.WithoutUI;
        }
        private void HideAll()
        {
            WinPopup.HideImmediate();
            StartPopup.HideImmediate();
            GameplayUI.SetActive(false);
        }

        private async UniTask TransitionTo(UIState state)
        {
            switch (state)
            {
                case UIState.SetupRequest:
                    await StartPopup.Show(Configuration.UIFadeDuration);

                    if (State == UIState.Win)
                        WinPopup.Hide(Configuration.UIFadeDuration);
                    break;

                case UIState.Gameplay:
                    GameplayUI.SetActive(true);
                    await StartPopup.Hide(Configuration.UIFadeDuration);
                    break;

                case UIState.Win:
                    await WinPopup.Show(Configuration.UIFadeDuration);
                    GameplayUI.SetActive(false);

                    await WinPopup.WaitForConfirmation();
                    break;

                default:
                    throw new ArgumentException($"Unable to perform transition to state {state}");
            }

            State = state;
        }
    }
}
