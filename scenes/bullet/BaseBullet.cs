using Godot;

namespace Game.Bullet;

public partial class BaseBullet : Area2D
{

	[Export] private float _speed;

	public CharacterBody2D _target;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
