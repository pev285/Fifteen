using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Core
{
    public interface IStorage
    {
        void ClearSavedData();

        void SaveSetup(GameSetup setup);
        bool TryRestoreSetup(out GameSetup setup);

        void SaveBoardArray(int[] board);
        bool TryRestoreBoardArray(out int[] board, int len);
    }
}
