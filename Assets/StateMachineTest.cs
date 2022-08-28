using TweensStateMachine;
using TweensStateMachine.Animations;
using TweensStateMachine.Animations.Move;
using TweensStateMachine.Runtime.Core;
using UnityEngine;

public class StateMachineTest : MonoBehaviour
{
    public TSMAnimation stateMachine;
    public bool stateOne;

    [ContextMenu("Add States")]
    private void AddStates()
    {
        stateMachine.AddState("First", new MoveAnimation());
        stateMachine.AddState("Second", new MoveAnimation(), new MoveAnimation());
    }

    private void Start()
    {
        stateMachine.AddTransition("First", "Second", () => !stateOne);
        stateMachine.AddTransition("Second", "First", () => stateOne);
        stateMachine.SetState("First");
    }

    private void Update()
    {
        stateMachine.Tick();
    }
}
