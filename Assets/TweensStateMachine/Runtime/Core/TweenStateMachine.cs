using System;
using System.Collections.Generic;
using System.Linq;
using TweensStateMachine.Animations;
using TweensStateMachine.Animations.Move;
using UnityEngine;

namespace TweensStateMachine.Runtime.Core
{
    public class TweenStateMachine : MonoBehaviour
    {
        public List<State> states = new List<State>();
        private Dictionary<string, List<Transition>> _transitions;
        private List<Transition> _currentTransitions;
        private AnimationBase _activeAnimation;
        private State _currentState;
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        private void Awake()
        {
            _transitions = new Dictionary<string, List<Transition>>();
            _currentTransitions = new List<Transition>();
        }

        [ContextMenu("Add Move State")]
        public void AddMoveState()
        {
            states.Add(new State("NewState", new MoveAnimation()));
        }
        
        [ContextMenu("Add Sequence with Move Animaiton")]
        public void AddSequenceWithMoveAnimation()
        {
            var sequence = new SequenceAnimation();
            sequence.AddAnimation(new MoveAnimation());
            states.Add(new State("NewState", sequence));
        }
        
        [ContextMenu("Add Sequence with 2 Move Animaiton")]
        public void AddSequenceWithTwoMoveAnimation()
        {
            var sequence = new SequenceAnimation();
            sequence.AddAnimation(new MoveAnimation());
            sequence.AddAnimation(new MoveAnimation());
            states.Add(new State("NewState", sequence, new MoveAnimation()));
        }

        public void Tick()
        {
            if (_currentState == null)
            {
                throw new InvalidOperationException(
                    "You need to set current state before using state machine. (Use SetState() before first Tick())");
            }
            var condition = GetTransition();
            if (condition != null)
                SetState(condition.To);
        }
        
        public void AddState(string stateName, params AnimationBase[] tweenTemplates)
        {
            AddState(new State(stateName, tweenTemplates));
        }
        
        public void SetState(string stateName)
        {
            if (states.All(x => x.stateName != stateName))
                throw new InvalidOperationException(
                    $"State {stateName} doesn't exist. You must add state before setting it active");
        
            var state = states.Find(x => x.stateName == stateName);
            if(state == _currentState)
                return;

            _activeAnimation?.ExecuteStateExitAction();

            _currentState = state;
            _currentTransitions = _transitions.TryGetValue(_currentState.stateName, out var transitions) ? transitions : EmptyTransitions;
        
            var anim = _currentState.OnEnter();
            _activeAnimation = anim;
            _activeAnimation.Start();
        }

        public void AddTransition(string from, string to, Func<bool> condition)
        {
            if (states.All(x => x.stateName != from))
                throw new InvalidOperationException(
                    $"State {from} doesn't exist. You must add state before adding transition for it");
            if (states.All(x => x.stateName != to))
                throw new InvalidOperationException(
                    $"State {to} doesn't exist. You must add state before adding transition for it");
        
            if (_transitions.TryGetValue(from, out List<Transition> transitions))
            {
                _transitions[from].Add(new Transition(to, condition));
            }
            else
            {
                _transitions[from] = new List<Transition>();
                _transitions[from].Add(new Transition(to, condition));
            }
        }
        
        private void AddState(State state)
        {
            if (states.Any(x => x.stateName == state.stateName))
            {
                throw new InvalidOperationException(
                    $"State {state.stateName} was already added to the Animator. There can't be 2 states with the same name");
            }
            states.Add(state);
        }
        
        private Transition GetTransition()
        {
            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
            return null;
        }
    }
}