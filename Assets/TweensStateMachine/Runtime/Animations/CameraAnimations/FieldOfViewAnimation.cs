using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.CameraAnimations
{
    [System.Serializable]
    public class FieldOfViewAnimation : TweenAnimation
    {
        public Camera target;
        public float value;

        protected override Tween StartInternal()
        {
            return target.DOFieldOfView(value, duration);
        }

        public override void GetValue()
        {
            if (target == null)
            {
                return;
            }

            value = target.fieldOfView;
        }
    }
}