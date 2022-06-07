using DG.Tweening;
using DG.Tweening.Core;

namespace TweensStateMachine.TweenTemplates
{
    public class TweenTemplateFloat : TweenTemplate
    {
        public DOGetter<float> getter;
        public DOSetter<float> setter;
        public float endValue;
        public float duration;

        public TweenTemplateFloat(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
        {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;
        }

        protected override Tween CreateTweenInternal()
        {
            return DOTween.To(getter, setter, endValue, duration);
        }
    }
}