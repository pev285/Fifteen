using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Level
    {
        public Board Board { get; private set; }
        public GameSetup GameSetup { get; private set; }

        private bool PuzzleCompleted;

        public Level(Board board)
        {
            Board = board;
            Board.Updated += CheckGameEnd;
        }

        //public async UniTask ShowBoard()
        //{
        //    await Board.Show();
        //}

        //public async UniTask HideBoard()
        //{
        //    await Board.Hide();
        //}

        public void SetupLevel(GameSetup setup)
        {
            GameSetup = setup;

            Board.Lock();
            Board.CreateNew(setup.BoardWidth, setup.BoardHeight);
        }

        public void RestoreLevel(GameSetup setup, int[] data)
        {
            GameSetup = setup;

            Board.Lock();
            Board.Restore(setup.BoardWidth, setup.BoardHeight, data);
        }

        public async UniTask ShuffleBoard()
        {
            await Board.Shuffle();
        }

        public async UniTask StartPlaying()
        {
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
