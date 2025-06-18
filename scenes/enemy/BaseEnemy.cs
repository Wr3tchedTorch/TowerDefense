using Game.Component;
using Game.Resources;
using Godot;

namespace Game.Enemy;

public partial class BaseEnemy : PathFollow2D
{
	public EnemyResource EnemyResource;

	private HealthComponent _healthComponent;

	private float PathIterationCountPerFrame => 2.5f * (EnemyResource.Speed / 300);

	public override void _Ready()
	{
		AddToGroup(nameof(BaseEnemy));
		
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.Death += OnDeath;
		
		_healthComponent.SetMaxHealth(EnemyResource.TotalHealth);
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
