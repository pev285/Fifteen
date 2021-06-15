using pe9.Fifteen.Common;
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

        private const string WidthKey = "Width";
        private const string HeightKey = "Height";

        public void ClearSavedData()
        {
            PlayerPrefs.DeleteKey(BoardKey);
            PlayerPrefs.DeleteKey(WidthKey);
            PlayerPrefs.DeleteKey(HeightKey);
        }

        public void SaveSetup(GameSetup setup)
        {
            PlayerPrefs.SetInt(WidthKey, setup.BoardWidth);
            PlayerPrefs.SetInt(HeightKey, setup.BoardHeight);
            PlayerPrefs.Save();
        }

        public bool TryRestoreSetup(out GameSetup setup)
        {
            var width = PlayerPrefs.GetInt(WidthKey, -1);
            var height = PlayerPrefs.GetInt(HeightKey, -1);

            if (width <= 0 || height <= 0)
            {
                setup = default(GameSetup);
                return false;
            }

            setup = new GameSetup(width, height, 0);
            return true;
        }

        public void SaveBoardArray(int[] data)
        {
            var boardString = string.Join(Separator.ToString(), data);

            PlayerPrefs.SetString(BoardKey, boardString);
            PlayerPrefs.Save();
        }

        public bool TryRestoreBoardArray(out int[] data, int len)
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
            return true;
        }
    }
}
