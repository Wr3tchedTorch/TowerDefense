using Game.Bullet;
using Godot;

namespace Game.State;

public partial class MoveToDirectionState : State
{
    [Signal] public delegate void DestinationReachedEventHandler();

    private Vector2 initialPosition;
    private Vector2 direction;
    private float maxDistance;
    private float speed = 200f;

    private bool isMoving = false;
    private BaseBullet parent;

    public override void Enter()
    {
        parent = GetOwner<BaseBullet>();

        if (parent == null)
        {
            GD.PrintErr("MoveToDirectionState must be a child of BaseBullet.");
            return;
        }

        initialPosition = parent.TurretPosition;
        maxDistance = parent.MaxDistance;
        speed = parent.Speed;
        direction = parent.GlobalPosition.DirectionTo(parent.Target.GlobalPosition);
        isMoving = true;
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!isMoving)
        {
            return;
        }
        var distance = speed * (float)delta;

        parent.GlobalPosition += direction * distance;

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
        return parent.GlobalPosition.DistanceTo(initialPosition) >= maxDistance;
    }
}
