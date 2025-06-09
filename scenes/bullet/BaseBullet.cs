using System.Linq;
using Game.scripts;
using Godot;

namespace Game.Bullet;

public partial class BaseBullet : Area2D
{
	public int Penetration { get; set; }
	public float Damage { get; set; }
	public float Speed { get; set; }
	public Node2D Target { get; set; }

	private int currentPenetration = 0;
	private IMovementComponent movementComponent;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;

		currentPenetration = Penetration;
		movementComponent = GetChildren().OfType<IMovementComponent>().FirstOrDefault();
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target))
		{
			QueueFree();
			return;
		}
		LookAt(Target.GlobalPosition);
		movementComponent.Move();
	}

	private void OnAreaEntered(Node2D area)
	{
		currentPenetration--;
		if (currentPenetration == 0)
		{
			QueueFree();
		}
	}

	private void OnDestinationReached()
	{
		QueueFree();
	}
}
