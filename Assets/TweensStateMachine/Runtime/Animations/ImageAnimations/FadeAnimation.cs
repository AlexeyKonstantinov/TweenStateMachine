using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine.UI;

namespace TweensStateMachine.Runtime.Animations.ImageAnimations
{
    [System.Serializable]
    public class FadeAnimation : TweenAnimation
    {
        public Image target;
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