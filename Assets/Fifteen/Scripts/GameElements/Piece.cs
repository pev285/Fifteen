using Cysharp.Threading.Tasks;
using DG.Tweening;
using pe9.Fifteen.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Piece : MonoBehaviour
    {
        public event Action<Vector2Int> Clicked;

        [SerializeField]
        private TextMesh Label;

        private Transform _transform;
        private Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;

                return _transform;
            }
        }

        public int LabelNumber { get; private set; }

        public void SetLabel(int label)
        {
            LabelNumber = label;
            var labelStr = label.ToString();

            gameObject.name = labelStr;
            Label.text = labelStr;
        }

        public void SetActive(bool on)
        {
            gameObject.SetActive(on);
        }

        public void SetPosition(Vector2 position)
        {
            Transform.position = position;
        }

        public async UniTask MovePosition(Vector2 position, float duration)
        {
            var tween = Transform.DOMove(position, duration);
            await TaskHelpers.WaitFor(tween);
        }

        private void OnMouseDown()
        {
            Vector2Int boardPosition = new Vector2Int((int)Transform.position.x, (int)Transform.position.y);
            Clicked?.Invoke(boardPosition);
        }
    }
}
