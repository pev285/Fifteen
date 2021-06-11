using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Common
{
    public class GameSetup
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }

        public GameSetup(int boardWidth, int boardHeight)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
        }
    }
}
