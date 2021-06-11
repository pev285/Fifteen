using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Level
    {
        private Board Board;

        private bool PuzzleCompleted;

        public Level(Board board)
        {
            Board = board;
            Board.Updated += CheckGameEnd;
        }

        public async UniTask ShowBoard()
        {
            await Board.Show();
        }

        public async UniTask HideBoard()
        {
            await Board.Hide();
        }


        public void SetupLevel(GameSetup setup)
        {
            Board.Lock();
            Board.Recreate(setup.BoardWidth, setup.BoardHeight);
        }

        public async UniTask StartNew()
        {
            await Board.Shuffle();
            Board.Unlock();

            PuzzleCompleted = false;
            await UniTask.WaitUntil(() => PuzzleCompleted == true);
        }

        private void CheckGameEnd()
        {
            if (Board.IsCorrectArrangement() == false)
                return;

            Board.Lock();
            PuzzleCompleted = true;
        }
    }
}
