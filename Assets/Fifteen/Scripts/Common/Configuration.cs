using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Common
{
    public static class Configuration 
    {
        public static float UIFadeDuration { get => 1.0f; }

        public static int BoardShuffleSteps { get => 3; }

        public static float PieceMoveDuration { get => 0.25f; }
        public static float ShuffleMoveDuration { get => 0.15f; }
    }
}
