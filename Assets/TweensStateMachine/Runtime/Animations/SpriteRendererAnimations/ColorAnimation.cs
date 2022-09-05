using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.SpriteRendererAnimations
{
    [System.Serializable]
    public class ColorAnimation : TweenAnimation
    {
        public SpriteRenderer target;
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