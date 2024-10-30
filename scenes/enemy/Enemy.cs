using Godot;

namespace Game.Enemy;

public partial class Enemy : PathFollow2D
{

	[Export] private float _movementSpeed = 180;

	private float PathIterationCountPerFrame => 2.5f * (_movementSpeed / 300);

	public override void _Ready()
	{
		AddToGroup(nameof(Enemy));
	}

	public override void _PhysicsProcess(double delta)
	{

		Progress += PathIterationCountPerFrame;
		Rotation = 0;
	}
}
