using DG.Tweening;
using UnityEngine;

namespace TweensStateMachine.Runtime.Core
{
    [System.Serializable]
    public abstract class AnimationBase
    {
        [SerializeField] internal float duration;
        [SerializeField] internal float delay;
        [SerializeField] internal Ease ease = Ease.Linear;
        [SerializeField] internal StateExitAction stateExitAction;

        internal abstract void ExecuteStateExitAction();
        internal abstract void Start();
        internal abstract void Complete();
        internal abstract void Kill();
        internal abstract float ElapsedPercentage();
    }
}