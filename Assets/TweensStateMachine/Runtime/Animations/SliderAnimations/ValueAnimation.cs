using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine.UI;

namespace TweensStateMachine.Runtime.Animations.SliderAnimations
{
    [System.Serializable]
    public class ValueAnimation : TweenAnimation
    {
        public Slider target;
        public float value;
        
        protected override Tween StartInternal()
        {
            return target.DOValue(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.value;
        }
    }
}