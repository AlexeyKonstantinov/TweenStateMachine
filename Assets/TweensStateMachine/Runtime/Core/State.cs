using UnityEngine;

namespace TweensStateMachine.Runtime.Core
{
    [System.Serializable]
    public class State
    {
        public string stateName;
        [SerializeReference]
        public SequenceAnimation sequence = new SequenceAnimation();

        public State(string stateName, params AnimationBase[] animations)
        {
            this.stateName = stateName;
            foreach (var animation in animations)
            {
                sequence.AddAnimation(animation);
            }
        }

        public AnimationBase OnEnter()
        {
            return sequence;
        }

        // public void AddAnimation(Animation animation)
        // {
        //     sequence.AddAnimation(animation);
        // }
    }
}