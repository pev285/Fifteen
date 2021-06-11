using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Level
    {
        private const int ShuffleSteps = 10;

        private Board Board;

        private bool PuzzleCompleted;

        public Level(Board board)
        {
            Board = board;
            Board.Updated += CheckGameEnd;
        }

        public async void StartNew(GameSetup setup)
        {
            await Board.Recreate(setup.BoardWidth, setup.BoardHeight);
            await Board.Shuffle(ShuffleSteps);

            PuzzleCompleted = false;
            await UniTask.WaitUntil(() => PuzzleCompleted == true);
        }

        private void CheckGameEnd()
        {

        }
    }
}
