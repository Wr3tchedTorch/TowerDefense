using Game.scripts;
using Godot;

public partial class GoToPointComponent : Node, IMovementComponent
{	
	[Signal] public delegate void DestinationReachedEventHandler();

	public Vector2 TargetPosition { get; set; }
	public float Speed { get; set; } = 100f;

	private Node2D parent;
	private bool isMoving = false;

	public override void _Ready()
	{
		parent = GetOwner<Node2D>();
	}

	public override void _Process(double delta)
	{
		if (!isMoving)
		{
			return;
		}
		var direction = parent.GlobalPosition.DirectionTo(TargetPosition);
		var distance = Speed * (float)delta;

		parent.GlobalPosition += direction * distance;

		if (parent.GlobalPosition.IsEqualApprox(TargetPosition))
		{
			EmitSignal(SignalName.DestinationReached);
		}
	}

	public void Stop()
	{
		isMoving = false;
	}

	public void Move()
	{
		isMoving = true;
	}
}
