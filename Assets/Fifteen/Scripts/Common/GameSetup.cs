using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Common
{
    public class GameSetup
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }

        public int ShuffleSteps { get; }

        public GameSetup(int boardWidth, int boardHeight, int shuffleSteps)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;

            ShuffleSteps = shuffleSteps;
        }
    }
}
