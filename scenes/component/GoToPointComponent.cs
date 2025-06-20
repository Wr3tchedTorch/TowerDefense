using Game.Bullet;
using Game.scripts;
using Godot;

public partial class GoToPointComponent : Node, IMovementComponent
{	
	[Signal] public delegate void DestinationReachedEventHandler();

	public Vector2 TargetPosition { get; set; }
	public float Speed { get; set; }

	private BaseBullet parent;
	private bool isMoving = false;
	const float REACH_THRESHOLD = 2.0f; // Adjust as needed

	public override void _Ready()
	{
		parent = GetOwner<BaseBullet>();

		Speed = parent.Speed;
	}

    public override void _PhysicsProcess(double delta)
    {
        if (!isMoving)
		{
			return;
		}
		var direction = parent.GlobalPosition.DirectionTo(TargetPosition);
		var distance = Speed * (float)delta;

		parent.GlobalPosition += direction * distance;
		
		var distanceToTarget = parent.GlobalPosition.DistanceTo(TargetPosition);
		if (distanceToTarget <= distance)
		{
			parent.GlobalPosition = TargetPosition;
			isMoving = false;
			GD.Print("Reached target position.");
			EmitSignal(SignalName.DestinationReached);
		}
    }

	public void Stop()
	{
		isMoving = false;
	}

	public void Move(Vector2 targetPosition)
	{
		if (isMoving)
		{
			return;
		}

		TargetPosition = targetPosition;
		isMoving = true;
	}
}
