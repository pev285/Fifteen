using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Board : MonoBehaviour
    {
        public Action Updated;

        public Transform Transform { get; private set; }

        [SerializeField]
        private Transform Background;

        private Piece[] Cells;
        private PieceFactory PieceFactory;

        private int Width;
        private int Height;

        private bool IsBusy = false;

        private void Awake()
        {
            Transform = transform;    
        }

        public void Initialize(PieceFactory pieceFactory)
        {
            PieceFactory = pieceFactory;
        }

        public async UniTask Recreate(int width, int height)
        {
            if (IsBusy)
                await UniTask.WaitUntil(() => IsBusy == false);

            IsBusy = true;

            Width = width;
            Height = height;

            Clear();
            UpdateBackground(width, height);

            CreatePieces(width, height);
            IsBusy = false;
        }

        public void Clear()
        {
            if (Cells == null)
                return;

            for (int i = 0; i < Cells.Length; i++) 
            {
                if (Cells[i] == null)
                    continue;

                Cells[i].Clicked -= Piece_Clicked;
                PieceFactory.GiveBack(Cells[i]);
            }

            Cells = null;
        }

        public async UniTask Shuffle()
        {

        }

        private void UpdateBackground(int width, int height)
        {
            Transform.position = new Vector3(0.5f * width, 0.5f * height, 0);
            Background.localScale = new Vector3(width, height, 1);
        }

        private void CreatePieces(int width, int height)
        {
            int len = width * height;
            Cells = new Piece[len];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (y == 0 && x == width - 1)
                        continue;
                    int index = CellIndex(x, y);
                    int label = CellLabel(x, y);

                    var piece = PieceFactory.GetPiece();
                    Cells[index] = piece;

                    piece.SetLabel(label.ToString());
                    piece.SetPosition(new Vector2(x, y));

                    piece.Clicked += Piece_Clicked;
                }
        }

        private int CellLabel(int x, int y)
        {
            return (Height - y - 1) * Width + x + 1;
        }

        private int CellIndex(int x, int y)
        {
            return y * Width + x;
        }

        private async void Piece_Clicked(Vector2Int position)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            Vector2Int targetPosition;

            if (FindEmptyNeighbor(position, out targetPosition) == false)
                return;

            await MovePiece(position, targetPosition);
            Updated?.Invoke();

            IsBusy = false;
        }

        private async UniTask MovePiece(Vector2Int position, Vector2Int targetPosition)
        {
            var index = CellIndex(position.x, position.y);
            var targetIndex = CellIndex(targetPosition.x, targetPosition.y);

            Cells[targetIndex] = Cells[index];
            Cells[index] = null;

            await Cells[targetIndex].MovePosition(targetPosition);
        }

        private bool FindEmptyNeighbor(Vector2Int position, out Vector2Int neighbor)
        {
            neighbor = new Vector2Int(position.x, position.y + 1);
            if (IsPositionAvailable(neighbor))
                return true;

            neighbor = new Vector2Int(position.x, position.y - 1);
            if (IsPositionAvailable(neighbor))
                return true;

            neighbor = new Vector2Int(position.x + 1, position.y);
            if (IsPositionAvailable(neighbor))
                return true;

            neighbor = new Vector2Int(position.x - 1, position.y);
            if (IsPositionAvailable(neighbor))
                return true;

            return false;
        }

        private bool IsPositionAvailable(Vector2Int position)
        {
            if (position.x < 0 || position.x >= Width)
                return false;

            if (position.y < 0 || position.y >= Height)
                return false;

            var index = CellIndex(position.x, position.y);

            if (Cells[index] == null)
                return true;

            return false;
        }
    }
}
