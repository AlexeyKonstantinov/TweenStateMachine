using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TweensStateMachine.Runtime.Animations.ImageAnimations
{
    [System.Serializable]
    public class FillAmountAnimation : TweenAnimation
    {
        public Image target;
        public float value;
        
        protected override Tween StartInternal()
        {
            return target.DOFillAmount(value, duration);
        }

        public override void GetValue()
        {
            if (target == null) {
                return;
            }

            value = target.fillAmount;
        }
    }
    
    [System.Serializable]
    public class GradientAnimation : TweenAnimation
    {
        public Image target;
        public Gradient value;
        
        protected override Tween StartInternal()
        {
            return target.DOGradientColor(value, duration);
        }

        public override void GetValue()
        {
        }
    }
}