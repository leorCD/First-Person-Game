using Godot;
using System;
using System.Collections.Generic;

public partial class MovementStateMachine : Node
{
    [Export] private MovementState _defaultState;
    private readonly Dictionary<Type, MovementState> _states = new();
    public MovementState CurrentState {get; private set;}

    public void Init(PlayerController player)
    {
        foreach (Node node in this.GetChildren()) // populate _states dictionary
        {
            if (node is MovementState state)
            {
                _states[state.GetType()] = state;
                state.Init(player);
            }
        }

        if (_defaultState != null)
        {
            CurrentState = _defaultState;
            _defaultState.Enter();
        }
        else
        {
            GD.Print("[MSM] No default state set");
        }
    }



    // this way its impossible to have typos if we just use class names directly
    public bool ChangeState<T>() where T : MovementState 
    {
        if (!_states.TryGetValue(typeof(T), out var nextState)) return false;
        if (nextState == CurrentState || !nextState.CanEnter()) return false;

        if (CurrentState != null) CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
        return true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState == null) return;

        CurrentState.PhysicsUpdate(delta);
    }

}
