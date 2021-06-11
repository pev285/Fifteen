using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Level
    {
        private Board Board;

        public Level(Board board)
        {
            Board = board;
            Board.Updated += CheckGameEnd;
        }

        public async void StartNew(GameSetup setup)
        {
            await Board.Recreate(setup.BoardWidth, setup.BoardHeight);
        }

        private void CheckGameEnd()
        {

        }
    }
}
