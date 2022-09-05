using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TweensStateMachine.Runtime.Animations.ImageAnimations
{
    [System.Serializable]
    public class ColorAnimation : TweenAnimation
    {
        public Image target;
        public Color value;
        
        protected override Tween StartInternal()
        {
            return target.DOColor(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.color;
        }
    }
}