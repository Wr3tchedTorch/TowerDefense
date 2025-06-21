using Game.State;
using Godot;

namespace Game.Bullet;

public partial class BaseBullet : Area2D
{
	public int Penetration { get; set; }
	public float Damage { get; set; }
	public float Speed { get; set; }
	public float MaxDistance { get; set; }
	public Node2D Target { get; set; }	

	private int currentPenetration = 0;
	private StateMachine stateMachine;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;

		currentPenetration = Penetration;
		stateMachine.SwitchTo("MoveToDirectionState");
	}
	
	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target))
		{			
			QueueFree();
			return;
		}
		LookAt(Target.GlobalPosition);
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
		QueueFree();
	}
}
