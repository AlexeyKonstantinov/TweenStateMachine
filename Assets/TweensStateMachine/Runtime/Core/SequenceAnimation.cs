using System;
using System.Collections.Generic;
using UnityEngine;

namespace TweensStateMachine.Runtime.Core
{
    [System.Serializable]
    public class SequenceAnimation : AnimationBase
    {
        [SerializeReference]
        public List<AnimationBase> animations = new List<AnimationBase>();

        public SequenceAnimation()
        {
            stateExitAction = StateExitAction.Continue;
        }

        public void AddAnimation(AnimationBase animation)
        {
            animations.Add(animation);
        }

        public void RemoveAnimation(AnimationBase animation)
        {
            animations.Remove(animation);
        }

        internal override void ExecuteStateExitAction()
        {
            if(stateExitAction == StateExitAction.Kill)
                Kill();
            else if(stateExitAction == StateExitAction.Complete)
                Complete();
            else
            {
                foreach (var animation in animations)
                {
                    animation.ExecuteStateExitAction();
                }
            }
        }

        internal sealed override void Start()
        {
            foreach (var animation in animations)
            {
                animation.Start();
            }
        }

        internal sealed override void Complete()
        {
            foreach (var animation in animations)
            {
                animation.Complete();
            }
        }

        internal sealed override void Kill()
        {
            foreach (var animation in animations)
            {
                animation.Kill();
            }
        }

        internal sealed override float ElapsedPercentage()
        {
            throw new NotImplementedException();
        }
    }
}