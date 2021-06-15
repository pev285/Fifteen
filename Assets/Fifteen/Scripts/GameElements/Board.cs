using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
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

        public void CreateNew(int width, int height)
        {
            CreateDefaultBoard(width, height);
            SetCorrectArrangement();
        }

        public void Restore(int width, int height, int[] data)
        {
            CreateDefaultBoard(width, height);
            SetBoardState(data);
        }

        public int[] GetBoardState()
        {
            var data = new int[Width * Height];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int index = InternalIndex(x, y);
                    int externalIndex = ExternalValue(x, y) - 1;

                    int value;

                    if (Cells[index] == null)
                        value = -1;
                    else
                        value = Cells[index].LabelNumber;

                    data[externalIndex] = value;
                }

            return data;
        }

        private void SetBoardState(int[] data)
        {
            if (data.Length != Width * Height)
                throw new ArgumentException($"Incorrect board data array length");

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int index = InternalIndex(x, y);
                    int externalIndex = ExternalValue(x, y) - 1;

                    int value = data[externalIndex];

                    if (value < 0)
                    {
                        ReturnPiece(Cells[index]);
                        Cells[index] = null;
                    }
                    else
                    {
                        Cells[index].SetLabel(value);
                    }
                }
        }

        public async UniTask Shuffle()
        {
            int counter = 0;
            var emptyPosition = FindEmptyPosition();
            var previousEmptyPosition = emptyPosition;

            while (counter < Configuration.BoardShuffleSteps)
            {
                int dirIndex = UnityEngine.Random.Range(0, Directions.Length);
                var direction = Directions[dirIndex];

                var sourcePosition = emptyPosition + direction;

                if (sourcePosition != previousEmptyPosition && IsInBoard(sourcePosition))
                {
                    await MovePiece(sourcePosition, emptyPosition, Configuration.ShuffleMoveDuration);

                    previousEmptyPosition = emptyPosition;
                    emptyPosition = sourcePosition;

                    counter++;
                }
            }
        }

        public bool IsCorrectArrangement()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int index = InternalIndex(x, y);
                    int label = ExternalValue(x, y);

                    if (Cells[index] == null)
                        continue;

                    if (Cells[index].LabelNumber != label)
                        return false;
                }

            return true;
        }


        /*
         * Returns the natural cell value for the game - 
         *      top most table line have numbers from 1 to Width,  
         *      the second line have numbers from Width+1 to 2*Width, etc
         * x is indexed from left to right
         * y is indexed form bottom to top
         */
        private int ExternalValue(int x, int y)
        {
            return (Height - y - 1) * Width + x + 1;
        }

        /*
         * Retruns: Position of an element in the Cells array
         * x is indexed from left to right
         * y is indexed form bottom to top
         */
        private int InternalIndex(int x, int y)
        {
            return y * Width + x;
        }

        private void Clear()
        {
            if (Cells == null)
                return;

            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] == null)
                    continue;

                ReturnPiece(Cells[i]);
            }

            Cells = null;
        }

        private void CreateDefaultBoard(int width, int height)
        {
            Width = width;
            Height = height;

            Clear();
            UpdateBackground(width, height);

            CreatePieces(width, height);
        }


        private Vector2Int FindEmptyPosition()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (Cells[InternalIndex(x, y)] == null)
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
                    CreatePiece(x, y);
        }

        private void ReturnPiece(Piece piece)
        {
            piece.Clicked -= TryMovePiece;
            PieceFactory.GiveBack(piece);
        }

        private void CreatePiece(int x, int y)
        {
            int index = InternalIndex(x, y);
            var piece = PieceFactory.GetPiece();

            Cells[index] = piece;

            piece.SetPosition(new Vector2(x, y));
            piece.Clicked += TryMovePiece;
        }

        private void SetCorrectArrangement()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int index = InternalIndex(x, y);

                    if (y == 0 && x == Width - 1)
                    {
                        ReturnPiece(Cells[index]);
                        Cells[index] = null;

                        continue;
                    }

                    int label = ExternalValue(x, y);
                    Cells[index].SetLabel(label);
                }
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

            await MovePiece(position, targetPosition, Configuration.PieceMoveDuration);
            IsBusy = false;

            Updated?.Invoke();
        }

        private async UniTask MovePiece(Vector2Int position, Vector2Int targetPosition, float duration)
        {
            var index = InternalIndex(position.x, position.y);
            var targetIndex = InternalIndex(targetPosition.x, targetPosition.y);

            Cells[targetIndex] = Cells[index];
            Cells[index] = null;

            await Cells[targetIndex].MovePosition(targetPosition, duration);
        }

        private bool FindEmptyNeighbor(Vector2Int position, out Vector2Int neighbor)
        {
            foreach (var direction in Directions)
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

            var index = InternalIndex(position.x, position.y);

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
