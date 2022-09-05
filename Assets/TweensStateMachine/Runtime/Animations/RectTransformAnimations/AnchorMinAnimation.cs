using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.RectTransformAnimations
{
    [System.Serializable]
    public class AnchorMinAnimation : TweenAnimation
    {
        public RectTransform target;
        public Vector2 value;
        
        protected override Tween StartInternal()
        {
            return target.DOAnchorMin(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }
            
            value = target.anchorMin;
        }
    }
}