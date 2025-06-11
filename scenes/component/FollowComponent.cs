using Game.scripts;
using Godot;
using System;

public partial class FollowComponent : Node, IMovementComponent
{
	public Node2D Target { get; set; }
	public float Speed { get; set; } = 100f;
	public float Velocity { get; set; } = 100f;

	private bool isMoving = false;

    public override void _Ready()
	{
		base._Ready();
	}

    public void Move(Vector2 targetPosition)
	{
		throw new NotImplementedException();
	}

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
