using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.TransformAnimations
{
    [System.Serializable]
    public class LocalMoveXAnimation : TweenAnimation
    {
        public Transform target;
        public float value;

        protected override Tween StartInternal()
        {
            return target.DOLocalMoveX(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.localPosition.x;
        }
    }
}