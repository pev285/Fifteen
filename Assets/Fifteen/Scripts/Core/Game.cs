using Cysharp.Threading.Tasks;
using pe9.Fifteen.Common;
using pe9.Fifteen.GameElements;
using pe9.Fifteen.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Core
{
    public class Game : MonoBehaviour
    {
        private UIHub UIHub;

        private Camera Camera;
        private Transform CameraTransform;

        private Level Level;

        public void Initialize(Camera camera, UIHub ui, Level level)
        {
            UIHub = ui;
            Level = level;

            Camera = camera;
            CameraTransform = Camera.GetComponent<Transform>();

        }

        public  async void StartGame()
        {
            await GameCycle();
        }

        private async UniTask GameCycle()
        {
            var setup = await UIHub.ShowStartGamePopup();

            SetupCamera(setup);
            Level.StartNew(setup);
            

        }

        private void SetupCamera(GameSetup setup)
        {
            var halfWidth = 0.5f * setup.BoardWidth;
            var halfHeight = 0.5f * setup.BoardHeight;

            CameraTransform.position = new Vector3(halfWidth, halfHeight, -10);

            var cameraSize = Screen.height * halfWidth / Screen.width;
            Camera.orthographicSize = cameraSize;

            //-- TODO: support different board height, if it's needed ---
        }
    }
}
