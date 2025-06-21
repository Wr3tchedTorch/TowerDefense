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
        if (CurrentState != null && CurrentState.Name == stateName)
        {
            GD.Print($"Already in state '{stateName}'. No switch needed.");
            return;
        }
        if (string.IsNullOrEmpty(stateName))
        {
            GD.PrintErr("State name cannot be null or empty.");
            return;
        }        
        if (!States.TryGetValue(stateName, out State value))
        {
            GD.PrintErr($"State '{stateName}' not found in the state machine.");
            return;
        }
        CurrentState?.Exit();
        RemoveChild(CurrentState);

        CurrentState = value;
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
