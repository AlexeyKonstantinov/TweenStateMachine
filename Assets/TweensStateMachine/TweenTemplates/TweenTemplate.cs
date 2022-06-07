using DG.Tweening;

namespace TweensStateMachine.TweenTemplates
{
    [System.Serializable]
    public abstract class TweenTemplate
    {
        internal Ease ease = Ease.Linear;
        internal float delay = 0;
        internal int loops = 1;

        public Tween CreateTween()
        {
            return CreateTweenInternal().SetEase(ease).SetDelay(delay).SetLoops(loops);
        }

        protected abstract Tween CreateTweenInternal();
    }
}