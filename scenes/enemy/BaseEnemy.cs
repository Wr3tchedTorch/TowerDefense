using Game.Component;
using Game.Resources;
using Godot;

namespace Game.Enemy;

public partial class BaseEnemy : PathFollow2D
{
	[ExportCategory("Dependencies")]
	[Export] private ProgressBar healthBar;
	[Export] private HealthComponent healthComponent;

	public EnemyResource EnemyResource;

	private float PathIterationCountPerFrame => 2.5f * (EnemyResource.Speed / 300f);

	public override void _Ready()
	{
		AddToGroup(nameof(BaseEnemy));
		
		healthComponent.Initialize(EnemyResource.TotalHealth, healthBar);
		healthComponent.Death += OnDeath;
	}

	public override void _PhysicsProcess(double delta)
	{
		Progress += PathIterationCountPerFrame;
		Rotation = 0;		
	}

	private void OnDeath()
	{
		QueueFree();
	}
}
