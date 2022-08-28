using DG.Tweening;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

namespace TweensStateMachine.Animations.Scale
{
    [System.Serializable]
    public class ScaleUniformlyAnimation : TweenAnimation
    {
        public Transform target;
        public float value;

        protected override Tween StartInternal()
        {
            return target.DOScale(value, duration);
        }

        public override void GetValue()
        {
            throw new System.NotImplementedException();
        }
    }
}