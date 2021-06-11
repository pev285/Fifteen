using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe9.Fifteen.Common
{
    public class TaskHelpers : MonoBehaviour
    {
        public static async UniTask WaitFor(Tweener tween)
        {
            await UniTask.WaitWhile(() => tween.active && tween.IsPlaying());
        }
    }
}
