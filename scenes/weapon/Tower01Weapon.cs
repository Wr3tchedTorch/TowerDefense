using Game.Bullet;
using Game.Tower;
using Godot;

public partial class Tower01Weapon : Node2D
{
	[Export] private BaseBullet bullet;

	private BaseTower parent;
	private Node2D target = null;

	private string TierAnimationName => $"level_0{(int)parent.CurrentTowerAttributesResource.Tier + 1}";

	public override void _Ready()
	{
		parent = GetOwner<BaseTower>();

		UpdateAnimation(TierAnimationName);
	}

	public override void _Process(double delta)
	{
		if (target == null || !IsInstanceValid(target))
		{
			return;
		}
		LookAt(target.GlobalPosition);
	}

	public void UpdateAnimation(string animationName)
	{
		var animatedSprite2D = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		if (animatedSprite2D == null)
		{
			GD.PrintErr("Tower01Weapon (ln 10): No AnimatedSprite2D found.");
			return;
		}
		animatedSprite2D.Animation = animationName;
		animatedSprite2D.Frame = 0;
	}
	
	private void OnTargetChanged(Node2D _, Node2D newTarget)
	{
		target = newTarget;
	}
}
