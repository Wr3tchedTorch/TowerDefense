using System.Collections.Generic;
using Godot;

namespace Game.State;

public partial class StateMachine : Node
{
    public Dictionary<string, State> States { get; private set; } = [];

    public State CurrentState { get; private set; } = null;

    public override void _Ready()
    {
        AddStates();
    }

    public void AddStates()
    {
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is State state && !States.ContainsKey(state.Name))
            {
                States[state.Name] = state;
                if (CurrentState == null)
                {
                    CurrentState = state;
                    continue;
                }
                RemoveChild(state);
            }
        }
        CurrentState.Enter();
    }

    public void SwitchTo(string stateName)
    {
        if (!States.ContainsKey(stateName))
        {
            GD.PrintErr($"State '{stateName}' not found in the state machine.");
            return;
        }
        CurrentState?.Exit();
        RemoveChild(CurrentState);

        CurrentState = States[stateName];
        CurrentState.Enter();
    }

    public override void _Process(double delta)
    {
        CurrentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.PhysicsUpdate(delta);
    }

    public override void _Input(InputEvent inputEvent)
    {
        CurrentState?.HandleInput(inputEvent);
    }
}
