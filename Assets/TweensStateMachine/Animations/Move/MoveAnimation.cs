using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Animations.Move
{
    [System.Serializable]
    public class MoveAnimation : TweenAnimation
    {
        public Transform target;
        public Vector3 value;

        protected override Tween StartInternal()
        {
            return target.DOMove(value, duration);
        }

        public override void GetValue()
        {
            if (target == null)
            {
                Debug.Log("TARGET NULL!");
                return;
            }

            value = target.position;
        }
    }
}