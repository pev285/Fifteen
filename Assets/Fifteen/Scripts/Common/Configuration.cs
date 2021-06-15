using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Common
{
    public static class Configuration
    {
        public static float UIFadeDuration { get => 1.0f; }

        public static float PieceMoveDuration { get => 0.25f; }
        public static float ShuffleMoveDuration { get => 0.1f; }

        public static int[][] SetupPresets { get => _setupPresets; }

        private static int[][] _setupPresets = {
            new int[] { 3, 3, 20 },
            new int[] { 4, 4, 30 },
            new int[] { 5, 5, 50 },
            new int[] { 3, 5, 40 },
            new int[] { 5, 4, 50 },
        };
    }
}
