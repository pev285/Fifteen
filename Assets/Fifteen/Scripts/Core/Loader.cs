using Cysharp.Threading.Tasks;
using pe9.Fifteen.GameElements;
using pe9.Fifteen.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace pe9.Fifteen.Core
{
    public class Loader : MonoBehaviour
    {
        public const string PiecePath = "Prefabs/Piece";
        public const string UIPath = "Prefabs/UICanvas";

        public const string GamePath = "Prefabs/Game";
        public const string BoardPath = "Prefabs/Board";

        public Camera Camera;
        
        private async void Start()
        {
            await LoadSceneObject();
            Destroy(gameObject);
        }

        private async UniTask LoadSceneObject()
        {
            var pieceObject = await LoadPrefab(PiecePath);
            var piece = pieceObject.GetComponent<Piece>();

            var board = await InstantiatePrefab<Board>(BoardPath);
            var pieceFactory = new PieceFactory(piece, board.Transform);

            board.Initialize(pieceFactory);
            var level = new Level(board);

            var uihub = await InstantiatePrefab<UIHub>(UIPath);
            var game = await InstantiatePrefab<Game>(GamePath);

            game.Initialize(Camera, uihub, level);
            game.StartGame();
        }

        private async UniTask<T> InstantiatePrefab<T>(string path)
        {
            var obj = await InstantiatePrefabObject(path);
            var component = obj.GetComponent<T>();

            return component;
        }

        private async UniTask<GameObject> InstantiatePrefabObject(string path)
        {
            var prefab = await LoadPrefab(path);
            var obj = Instantiate(prefab);

            return obj;
        }

        private async UniTask<GameObject> LoadPrefab(string path)
        {
            var request = Resources.LoadAsync(path, typeof(GameObject));
            await request;

            return request.asset as GameObject;
        }
    }
}
