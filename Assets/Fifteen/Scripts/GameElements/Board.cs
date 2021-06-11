using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Board : MonoBehaviour
    {
        private const int ShuffleSteps = 10;
        private const float ShuffleMoveDuration = 0.15f;


        public Action Updated;

        public Transform Transform { get; private set; }
        public bool IsBusy { get; private set; } = false;

        [SerializeField]
        private Transform Background;

        private readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        private Piece[] Cells;
        private PieceFactory PieceFactory;

        private int Width;
        private int Height;


        private void Awake()
        {
            Transform = transform;    
        }

        //-- TODO: Add Show/Hide animation?? --
        public async UniTask Show()
        {
            gameObject.SetActive(true);
        }

        public async UniTask Hide()
        {
            gameObject.SetActive(false);
        }

        public void Lock()
        {
            IsBusy = true;
        }

        public void Unlock()
        {
            IsBusy = false;
        }

        public void Initialize(PieceFactory pieceFactory)
        {
            PieceFactory = pieceFactory;
        }

        public void Recreate(int width, int height)
        {
            Width = width;
            Height = height;

            Clear();
            UpdateBackground(width, height);

            CreatePieces(width, height);
        }

        public void Clear()
        {
            if (Cells == null)
                return;

            for (int i = 0; i < Cells.Length; i++) 
            {
                if (Cells[i] == null)
                    continue;

                Cells[i].Clicked -= TryMovePiece;
                PieceFactory.GiveBack(Cells[i]);
            }

            Cells = null;
        }


        public async UniTask Shuffle()
        {
            int counter = 0;
            var emptyPosition = EmptyPosition();
            var previousEmptyPosition = emptyPosition;

            while(counter < ShuffleSteps)
            {
                int dirIndex = UnityEngine.Random.Range(0, Directions.Length);
                var direction = Directions[dirIndex];

                var sourcePosition = emptyPosition + direction;
                
                if (sourcePosition != previousEmptyPosition && IsInBoard(sourcePosition))
                {
                    await MovePiece(sourcePosition, emptyPosition, ShuffleMoveDuration);

                    previousEmptyPosition = emptyPosition;
                    emptyPosition = sourcePosition;

                    counter++;
                }
            }
        }

        public Vector2Int EmptyPosition()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (Cells[CellIndex(x, y)] == null)
                        return new Vector2Int(x, y);

            throw new Exception("No empty cell in the board!");
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

                    piece.SetLabel(label);
                    piece.SetPosition(new Vector2(x, y));

                    piece.Clicked += TryMovePiece;
                }
        }

        public bool IsCorrectArrangement()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int index = CellIndex(x, y);
                    int label = CellLabel(x, y);

                    if (Cells[index] == null)
                        continue;

                    if (Cells[index].LabelNumber != label)
                        return false;
                }

            return true;
        }

        private int CellLabel(int x, int y)
        {
            return (Height - y - 1) * Width + x + 1;
        }

        private int CellIndex(int x, int y)
        {
            return y * Width + x;
        }

        private async void TryMovePiece(Vector2Int position)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            Vector2Int targetPosition;

            if (FindEmptyNeighbor(position, out targetPosition) == false)
            {
                IsBusy = false;
                return;
            }

            await MovePiece(position, targetPosition);
            IsBusy = false;

            Updated?.Invoke();
        }

        private async UniTask MovePiece(Vector2Int position, Vector2Int targetPosition, float duration = 0)
        {
            var index = CellIndex(position.x, position.y);
            var targetIndex = CellIndex(targetPosition.x, targetPosition.y);

            Cells[targetIndex] = Cells[index];
            Cells[index] = null;

            if (duration == 0)
                await Cells[targetIndex].MovePosition(targetPosition);
            else
                await Cells[targetIndex].MovePosition(targetPosition, duration);
        }

        private bool FindEmptyNeighbor(Vector2Int position, out Vector2Int neighbor)
        {
            foreach(var direction in Directions)
            {
                neighbor = position + direction;
                
                if (IsPositionAvailable(neighbor))
                    return true;
            }

            neighbor = Vector2Int.zero;
            return false;
        }

        private bool IsPositionAvailable(Vector2Int position)
        {
            if (IsInBoard(position) == false)
                return false;

            var index = CellIndex(position.x, position.y);

            if (Cells[index] == null)
                return true;

            return false;
        }

        private bool IsInBoard(Vector2Int position)
        {
            if (position.x < 0 || position.x >= Width)
                return false;

            if (position.y < 0 || position.y >= Height)
                return false;

            return true;
        }
    }
}
