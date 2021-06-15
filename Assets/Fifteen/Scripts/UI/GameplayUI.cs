using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pe9.Fifteen.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public Action RestartButtonClicked;

        [SerializeField]
        private Button RestartButton;

        public void SetActive(bool on)
        {
            gameObject.SetActive(on);
        }

        private void Awake()
        {
            RestartButton.onClick.AddListener(() => RestartButtonClicked?.Invoke());
        }
    }
}
