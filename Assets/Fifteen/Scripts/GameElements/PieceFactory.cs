using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace pe9.Fifteen.GameElements
{
    public class PieceFactory
    {
        private Piece PiecePrefab;
        private Transform Root;

        private List<Piece> Reserve = new List<Piece>();

        public PieceFactory(Piece prefab, Transform piecesRoot)
        {
            Root = piecesRoot;
            PiecePrefab = prefab;
        }

        public Piece GetPiece()
        {
            if (Reserve.Count == 0)
                return GameObject.Instantiate(PiecePrefab, Root);

            var lastIndex = Reserve.Count - 1;

            var piece = Reserve[lastIndex];
            Reserve.RemoveAt(lastIndex);

            piece.SetActive(true);
            return piece;
        }

        public void GiveBack(Piece piece)
        {
            if (piece == null)
                return;

            Reserve.Add(piece);
            piece.SetActive(false);
        }
    }
}

