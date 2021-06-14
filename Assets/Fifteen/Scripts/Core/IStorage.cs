using pe9.Fifteen.GameElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Core
{
    public interface IStorage
    {
        void SaveArrray(int[] board);
        bool TryRestoreArray(out int[] board, int len);
    }
}
