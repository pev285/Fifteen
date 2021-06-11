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
    public class StartPopup : MonoBehaviour
    {
        [SerializeField]
        private Button StartButton;

        [SerializeField]
        private TMP_Dropdown SizeChooser;

        private bool Submitted;
        private GameSetup GameSetup;

        private void Awake()
        {
            StartButton.onClick.AddListener(CheckSuccess);
        }

        private void CheckSuccess()
        {
            var index = SizeChooser.value;
            var option = SizeChooser.options[index].text;

            var substrs = option.Split(new char[] { 'x' });

            string wstring = substrs[0].Trim();
            string hstring = substrs[1].Trim();

            int width = int.Parse(wstring);
            int height = int.Parse(hstring);

            GameSetup = new GameSetup(width, height);
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
