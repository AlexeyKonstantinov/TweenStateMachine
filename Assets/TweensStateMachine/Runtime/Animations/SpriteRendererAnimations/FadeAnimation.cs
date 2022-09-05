using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.SpriteRendererAnimations
{
    [System.Serializable]
    public class FadeAnimation : TweenAnimation
    {
        public SpriteRenderer target;
        public float value;
        
        protected override Tween StartInternal()
        {
            return target.DOFade(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }
            
            value = target.color.a;
        }
    }
}