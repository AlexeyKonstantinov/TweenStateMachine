using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.TransformAnimations
{
    [System.Serializable]
    public class LocalMoveAnimation : TweenAnimation
    {
        public Transform target;
        public Vector3 value;

        protected override Tween StartInternal()
        {
            return target.DOLocalMove(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.localPosition;
        }
    }
}