using Godot;

namespace Game.Bullet;

public partial class BaseBullet : Area2D
{

	[Export] public float Damage { get; private set; }
	[Export] private float _speed = 400;

	public Node2D Target { get; set; }

	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;
	}

	public override void _Process(double delta)
	{

		if (!IsInstanceValid(Target))
			return;

		GlobalPosition += _speed * GetDirectionToPosition(Target.GlobalPosition) * (float)delta;
	}

	private Vector2 GetDirectionToPosition(Vector2 position)
	{
		return (position - GlobalPosition).Normalized();
	}

	private void OnAreaEntered(Node2D area)
	{
		QueueFree();
	}
}
