using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Animations.Move
{
    [System.Serializable]
    public class MoveXAnimation : TweenAnimation
    {
        public Transform target;
        public float value;

        protected override Tween StartInternal()
        {
            return target.DOMoveX(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.position.x;
        }
    }
}