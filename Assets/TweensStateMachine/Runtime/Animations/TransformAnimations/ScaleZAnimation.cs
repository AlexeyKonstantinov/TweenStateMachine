using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.TransformAnimations
{
    [System.Serializable]
    public class ScaleZAnimation : TweenAnimation
    {
        public Transform target;
        public float value;
        
        protected override Tween StartInternal()
        {
            return target.DOScaleZ(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }
            
            value = target.localScale.z;
        }
    }
}