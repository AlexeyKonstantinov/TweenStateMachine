using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.LightAnimations
{
    [System.Serializable]
    public class IntensityAnimation : TweenAnimation
    {
        public Light target;
        public float value;

        protected override Tween StartInternal()
        {
            return target.DOIntensity(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.intensity;
        }
    }
}