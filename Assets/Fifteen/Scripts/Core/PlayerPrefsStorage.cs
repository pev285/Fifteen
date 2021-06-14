using pe9.Fifteen.GameElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Core 
{
    public class PlayerPrefsStorage : IStorage
    {
        private const char Separator = '#';
        private const string BoardKey = "Board";


        //public void SaveSetup() { }
        //public bool TryRestoreSetup() { }

        public void SaveArrray(int[] data)
        {
            var boardString = string.Join(Separator.ToString(), data);

            PlayerPrefs.SetString(BoardKey, boardString);
            PlayerPrefs.Save();
        }

        public bool TryRestoreArray(out int[] data, int len)
        {
            data = Array.Empty<int>();

            var boardString = PlayerPrefs.GetString(BoardKey);
            var values = boardString.Split(Separator);

            if (values.Length != len)
                return false;

            var result = new int[len];

            for (int i = 0; i < len; i++)
            {
                int tmp;

                if (int.TryParse(values[i], out tmp) == false)
                    return false;

                result[i] = tmp;
            }

            data = result;
            return false;
        }
    }
}
