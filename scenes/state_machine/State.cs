using Godot;

namespace Game.State;

public abstract partial class State : Node
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }
    public virtual void HandleInput(InputEvent inputEvent) { }
}
