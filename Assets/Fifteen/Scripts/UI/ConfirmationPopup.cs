﻿using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pe9.Fifteen.UI
{
    public class ConfirmationPopup : BasePopup
    {
        [SerializeField]
        private Button ConfirmButton;

        private bool HaveConfirmation;

        protected override void Awake()
        {
            base.Awake();
            ConfirmButton.onClick.AddListener(Confirm);
        }

        private void Confirm()
        {
            HaveConfirmation = true;
        }

        public async UniTask WaitForConfirmation()
        {
            HaveConfirmation = false;

            await UniTask.WaitUntil(() => HaveConfirmation == true);
        }
    }
}
