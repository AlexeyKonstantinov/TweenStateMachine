using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetState1();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetState2();
            }
        }

        [ContextMenu("Set State_1")]
        public void SetState1()
        {
            SetState("State_1");
        }
        
        [ContextMenu("Set State_2")]
        public void SetState2()
        {
            SetState("State_2");
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