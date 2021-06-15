﻿using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class Level
    {
        public enum LevelState
        {
            Playing,
            Completed,
            Aborted,
        }

        public Board Board { get; private set; }
        public GameSetup GameSetup { get; private set; }

        public LevelState State { get; private set; }

        public Level(Board board)
        {
            Board = board;
            Board.Updated += CheckGameEnd;
        }

        public void SetupLevel(GameSetup setup)
        {
            GameSetup = setup;

            Board.Lock();
            Board.CreateNew(setup.BoardWidth, setup.BoardHeight, setup.ShuffleSteps);
        }

        public void RestoreLevel(GameSetup setup, int[] data)
        {
            GameSetup = setup;

            Board.Lock();
            Board.Restore(setup.BoardWidth, setup.BoardHeight, data);
        }

        public async UniTask ShuffleBoard()
        {
            await Board.Shuffle();
        }

        public async UniTask StartPlaying()
        {
            Board.Unlock();

            State = LevelState.Playing;
            await UniTask.WaitWhile(() => State == LevelState.Playing);
        }

        public void Abort()
        {
            State = LevelState.Aborted;
        }

        private void CheckGameEnd()
        {
            if (Board.IsCorrectArrangement() == false)
                return;

            Board.Lock();
            State = LevelState.Completed;
        }
    }
}
