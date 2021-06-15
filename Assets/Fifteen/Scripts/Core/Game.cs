using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using pe9.Fifteen.GameElements;
using pe9.Fifteen.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace pe9.Fifteen.Core
{
    public class Game : MonoBehaviour
    {
        private enum GameState
        {
            Dialogs,
            Gameplay,
            Exit
        }

        private UIHub UIHub;

        private Camera Camera;
        private Transform CameraTransform;

        private Level Level;
        private IStorage Storage;

        private GameState State;

        public void Initialize(Camera camera, UIHub ui, Level level)
        {
            State = GameState.Dialogs;

            UIHub = ui;
            UIHub.GameAbortRequested += AbortGame;

            Level = level;

            Camera = camera;
            CameraTransform = Camera.GetComponent<Transform>();

            Storage = new PlayerPrefsStorage();
        }

        public  async void StartGame()
        {
            //Storage.ClearSavedData();
            await GameCycle();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false && State == GameState.Gameplay)
                SaveGame();
        }

        private void SaveGame()
        {
            Storage.SaveSetup(Level.GameSetup);
            Storage.SaveBoardArray(Level.Board.GetBoardState());
        }

        private async UniTask GameCycle()
        {
            while (State != GameState.Exit)
            {
                await SetupNewSession();

                State = GameState.Gameplay;
                await Level.StartPlaying();

                Storage.ClearSavedData();
                State = GameState.Dialogs;

                if (Level.State == Level.LevelState.Aborted)
                    continue;

                await UIHub.Win();
            }
        }

        private async Task SetupNewSession()
        {
            GameSetup setup;
            int[] boardData;

            bool gameRestored = false;

            if (Storage.TryRestoreSetup(out setup) 
                && Storage.TryRestoreBoardArray(out boardData, setup.BoardWidth * setup.BoardHeight))
            {
                SetupCamera(setup);
                Level.RestoreLevel(setup, boardData);

                gameRestored = true;
            }
            else
            {
                setup = await UIHub.SetupRequest();

                SetupCamera(setup);
                Level.SetupLevel(setup);
            }

            await UIHub.Gameplay();

            if (gameRestored == false)
                await Level.ShuffleBoard();
        }

        private void SetupCamera(GameSetup setup)
        {
            var halfWidth = 0.5f * setup.BoardWidth;
            var halfHeight = 0.5f * setup.BoardHeight;

            CameraTransform.position = new Vector3(halfWidth, halfHeight, -10);

            var cameraSize = Screen.height * halfWidth / Screen.width;
            Camera.orthographicSize = cameraSize;

            //-- TODO: support very high boards ??? ---
        }

        private void AbortGame()
        {
            Level.Abort();
        }
    }
}
