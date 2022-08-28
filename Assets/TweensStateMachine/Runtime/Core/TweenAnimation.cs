using DG.Tweening;

namespace TweensStateMachine.Runtime.Core
{
    [System.Serializable]
    public abstract class TweenAnimation : AnimationBase
    {
        private Tween _tween;

        internal sealed override void ExecuteStateExitAction()
        {
            if(stateExitAction == StateExitAction.Kill)
                Kill();
            else if(stateExitAction == StateExitAction.Complete)
                Complete();
        }

        internal sealed override void Start()
        {
            _tween = StartInternal().SetEase(ease).SetDelay(delay);
        }

        internal sealed override void Complete()
        {
            _tween.Complete();
        }

        internal sealed override void Kill()
        {
            _tween.Kill();
        }

        internal sealed override float ElapsedPercentage()
        {
            return _tween.ElapsedPercentage();
        }
        
        protected abstract Tween StartInternal();
        public abstract void GetValue();
    }
}