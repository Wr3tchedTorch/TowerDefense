using Godot;
using TowerDefense.enums;

namespace Game.Bullet;

public partial class BaseBullet : Area2D
{
	public float Damage { get; set; }
	public float Speed  { get; set; }
	public TowerTier Tier  { get; set; }
	public Node2D Target { get; set; }

	private string TierAnimationName => $"level_0{(int)Tier + 1}";

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;

		UpdateSprite();
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target))
		{
			QueueFree();
			return;
		}
		GlobalPosition += Speed * GetDirectionToPosition(Target.GlobalPosition) * (float)delta;
	}

	private void UpdateSprite()
	{
		var animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = TierAnimationName;
		animatedSprite2D.Frame = 0;
		animatedSprite2D.Play();
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
