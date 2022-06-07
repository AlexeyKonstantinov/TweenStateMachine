using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace TweensStateMachine.TweenTemplates
{
    public class TweenTemplateVector2 : TweenTemplate
    {
        public DOGetter<Vector2> getter;
        public DOSetter<Vector2> setter;
        public Vector2 endValue;
        public float duration;

        public TweenTemplateVector2(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
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