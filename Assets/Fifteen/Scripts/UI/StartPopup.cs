using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace pe9.Fifteen.UI
{
    public class StartPopup : BasePopup
    {
        private const char Separator = 'x';

        [SerializeField]
        private Button StartButton;

        [SerializeField]
        private TMP_Dropdown SizeChooser;

        private bool Submitted;
        private GameSetup GameSetup;

        protected override void Awake()
        {
            base.Awake();

            var options = SizeChooser.options;
            options.Clear();

            foreach (var preset in Configuration.SetupPresets)
            {
                var option = new TMP_Dropdown.OptionData($"{preset[0]} {Separator} {preset[1]}");
                options.Add(option);
            }

            StartButton.onClick.AddListener(CheckSuccess);
        }

        private void CheckSuccess()
        {
            var index = SizeChooser.value;
            var preset = Configuration.SetupPresets[index];

            GameSetup = new GameSetup(preset[0], preset[1], preset[2]);
            Submitted = true;
        }

        public async UniTask<GameSetup> WaitForStartSubmit()
        {
            Submitted = false;
            await UniTask.WaitUntil(() => Submitted == true);

            return GameSetup;
        }

    }
}
