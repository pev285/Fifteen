using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Piece : MonoBehaviour
    {
        public event Action<Vector2Int> Clicked;

        private const float MoveDuration = 2.0f;

        [SerializeField]
        private TextMesh Label;

        private GameObject GO;
        private Transform Transform;

        private void Awake()
        {
            GO = gameObject;
            Transform = transform;
        }

        public void SetLabel(string label)
        {
            GO.name = label;
            Label.text = label;
        }

        public void SetActive(bool on)
        {
            GO.SetActive(on);
        }

        public void SetPosition(Vector2 position)
        {
            Transform.position = position;
        }

        public async UniTask MovePosition(Vector2 position, float duration = MoveDuration)
        {
            var tween = Transform.DOMove(position, duration);
            await UniTask.WaitWhile(() => tween.active && tween.IsPlaying());
        }

        private void OnMouseDown()
        {
            Vector2Int boardPosition = new Vector2Int((int)Transform.position.x, (int)Transform.position.y);
            Clicked?.Invoke(boardPosition);
            Debug.Log($"{Label.text} clicked at {boardPosition}");
        }
    }
}
