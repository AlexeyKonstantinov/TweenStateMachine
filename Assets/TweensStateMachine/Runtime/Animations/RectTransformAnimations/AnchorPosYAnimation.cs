using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Runtime.Animations.RectTransformAnimations
{
    [System.Serializable]
    public class AnchorPosYAnimation : TweenAnimation
    {
        public RectTransform target;
        public float value;
        
        protected override Tween StartInternal()
        {
            return target.DOAnchorPosY(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }
            
            value = target.anchoredPosition.y;
        }
    }
}