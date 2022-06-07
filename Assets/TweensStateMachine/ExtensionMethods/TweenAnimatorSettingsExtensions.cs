using DG.Tweening;
using TweensStateMachine.TweenTemplates;

namespace TweensStateMachine
{
    public static class TweenAnimatorSettingsExtensions
    {
        public static T SetEase<T>(this T tt, Ease ease) where T : TweenTemplate
        {
            tt.ease = ease;
            return tt;
        }
        
        public static T SetDelay<T>(this T tt, float delay) where T : TweenTemplate
        {
            tt.delay = delay;
            return tt;
        }
        
        public static T SetLoops<T>(this T tt, int loops) where T : TweenTemplate
        {
            tt.loops = loops;
            return tt;
        }
    }
}