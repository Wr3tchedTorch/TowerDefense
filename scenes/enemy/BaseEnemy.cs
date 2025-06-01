using Game.Component;
using Godot;

namespace Game.Enemy;

public partial class BaseEnemy : PathFollow2D
{
	
	[Export] private float _damage = 10;
	[Export] private float _movementSpeed = 180;

	private HealthComponent _healthComponent;

	private float PathIterationCountPerFrame => 2.5f * (_movementSpeed / 300);

	public override void _Ready()
	{
		AddToGroup(nameof(BaseEnemy));

		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.Death += OnDeath;
	}

	public override void _PhysicsProcess(double delta)
	{

		Progress += PathIterationCountPerFrame;
		Rotation = 0;
	}

	private void OnDeath()
	{
		QueueFree();

		GD.Print("OH NO! I JUST DIED");
	}
}
