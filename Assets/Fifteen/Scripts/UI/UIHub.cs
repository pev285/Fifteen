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
            WinPopup.HideImmediate();
            StartPopup.HideImmediate();
            GameplayUI.SetActive(false);
        }

        private async UniTask TransitionTo(UIState state)
        {
            switch (state)
            {
                case UIState.SetupRequest:
                    await StartPopup.Show(FadeDuration);

                    if (State == UIState.Win)
                        WinPopup.Hide(FadeDuration);


                    //var t1 = StartPopup.Show(FadeDuration);
                    //var t2 = WinPopup.Hide(FadeDuration);
                    //await UniTask.WhenAll(t1, t2);
                    break;

                case UIState.Gameplay:
                    GameplayUI.SetActive(true);

                    //if (State == UIState.SetupRequest)
                        await StartPopup.Hide(FadeDuration);
                    //else
                    //    await WinPopup.Hide(FadeDuration);
                    break;

                case UIState.Win:
                    await WinPopup.Show(FadeDuration);
                    GameplayUI.SetActive(false);

                    await WinPopup.WaitForConfirmation();
                    //await WinPopup.Hide(FadeDuration);
                    break;

                default:
                    throw new ArgumentException($"Unable to perform transition to state {state}");
            }

            State = state;
        }
    }
}
