using Game.Component;
using Game.Resources;
using Godot;
using TowerDefense.scripts.interfaces;

namespace Game.Enemy;

public partial class BaseEnemy : PathFollow2D
{
	[ExportCategory("Dependencies")]
	[Export] private ProgressBar healthBar;
	[Export] private HealthComponent healthComponent;

	public EnemyResource EnemyResource;
	public float PathIterationCountPerFrame => 2.5f * (EnemyResource.Speed / 300f);

	private Vector2 previousPosition = Vector2.Zero;

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

		previousPosition = GlobalPosition;
	}

	public Vector2 GetPositionInFrames(int framesQuantity)
	{
		var path2d = GetParent<Path2D>();
		var curve = path2d.Curve;

		var nextProgress = Progress + PathIterationCountPerFrame * framesQuantity;
		return curve.SampleBaked(nextProgress);
	}

	private void OnDeath()
	{
		QueueFree();
	}
}
