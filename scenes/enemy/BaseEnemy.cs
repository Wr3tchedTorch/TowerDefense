using Game.Component;
using Game.Resources;
using Godot;

namespace Game.Enemy;

public partial class BaseEnemy : PathFollow2D
{
	[Export] private ProgressBar progressBar;

	public EnemyResource EnemyResource;

	private HealthComponent _healthComponent;

	private float PathIterationCountPerFrame => 2.5f * (EnemyResource.Speed / 300f);

	public override void _Ready()
	{
		AddToGroup(nameof(BaseEnemy));
		
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.Initialize(EnemyResource.TotalHealth, progressBar);
		_healthComponent.Death += OnDeath;
	}

	public override void _PhysicsProcess(double delta)
	{
		GD.Print($"Enemy: {EnemyResource.Name}");
		GD.Print($"Enemy: {EnemyResource.Speed}");
		GD.Print($"Enemy: {EnemyResource.Speed / 300}");		

		Progress += PathIterationCountPerFrame;
		Rotation = 0;		
	}

	private void OnDeath()
	{
		QueueFree();

		GD.Print("OH NO! I JUST DIED");
	}
}
