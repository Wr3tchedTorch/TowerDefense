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

        if (movementComponent is not Node movementComponentNode)
        {
            GD.PrintErr("BaseBullet: Movement component is not set or not found.");
            return;
        }
		if (!movementComponentNode.HasSignal("DestinationReached"))
		{
			GD.PrintErr("BaseBullet: Movement component does not have DestinationReached signal.");
			return;
		}
        movementComponentNode.Connect("DestinationReached", Callable.From(OnDestinationReached));
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target))
		{
			QueueFree();
			return;
		}
		LookAt(Target.GlobalPosition);
		movementComponent.Move(Target.GlobalPosition);
	}

	private void OnAreaEntered(Node2D area)
	{
		currentPenetration--;
		if (currentPenetration <= 0)
		{
			QueueFree();
		}
	}

	private void OnDestinationReached()
	{
		GD.PrintErr("BaseBullet: destination reached.");
		QueueFree();
	}
}
