using Game.Bullet;
using Godot;

namespace Game.Component;

public partial class HitboxComponent : Area2D
{

	[Export] private HealthComponent _healthComponent;

	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Node2D area)
	{

		if (area is BaseBullet bullet)
		{
			GD.Print($"Taking damage from bullet: {bullet.Name} | Damage: {bullet.Damage}");
			_healthComponent.Damage(bullet.Damage);
		}
	}
}
