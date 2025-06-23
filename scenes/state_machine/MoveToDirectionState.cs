using Godot;
using TowerDefense.scripts.interfaces;

namespace Game.State;

public partial class MoveToDirectionState : State
{
    [Signal] public delegate void DestinationReachedEventHandler();

    public override Node2D Parent { get; set; } = null;

    private IMovableToDirection movableToDirectionParent;
    private bool isMoving = false;

    public override void Enter()
    {
        movableToDirectionParent = Parent as IMovableToDirection;

        if (movableToDirectionParent == null)
        {
            GD.PrintErr("MoveToDirectionState must be a child of a class implementing IMovableToDirection.");
            return;
        }
        isMoving = true;
    }

    public override void PhysicsUpdate(double delta)
    {
        bool areAttributesValid = movableToDirectionParent.InitialPosition != Vector2.Zero &&
                                    movableToDirectionParent.MovementDirection != Vector2.Zero &&
                                    movableToDirectionParent.Speed > 0;
        if (!isMoving || !areAttributesValid)
        {
            return;
        }
        var distance = movableToDirectionParent.Speed * (float)delta;
        Parent.GlobalPosition += movableToDirectionParent.MovementDirection * distance;

        if (IsOutOfRange())
        {
            GD.Print("Reached target position.");
            EmitSignal(SignalName.DestinationReached);
            isMoving = false;
        }
    }

    public override void Exit()
    {
        isMoving = false;
    }

    private bool IsOutOfRange()
    {
        return Parent.GlobalPosition.DistanceTo(movableToDirectionParent.InitialPosition) >= movableToDirectionParent.MaxMovementDistance;
    }
}
