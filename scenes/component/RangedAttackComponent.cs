using Game.Bullet;
using Game.Unit;
using Godot;

namespace Game.Component;

public partial class RangedAttackComponent : Node
{
	[Export] private Marker2D _bulletSpawnPoint;

	[Export] private UpgradeComponent _upgradeComponent;

	private RangedUnit parent;
	private bool canShoot = true;

	private float FireRateDelay
	{
		get
		{
			float percentage = Mathf.Clamp(_upgradeComponent.BulletResource.FireRatePercentage, 0f, 100f);
			float minDelay = _upgradeComponent.BulletResource.MinFireRateDelay;
			float maxDelay = _upgradeComponent.BulletResource.MaxFireRateDelay;
			
			float t = 1f - Mathf.Pow(percentage / 100f, 2f);
			return Mathf.Lerp(minDelay, maxDelay, t);
		}
	}

	public override void _Ready()
	{		
		parent = GetParent<RangedUnit>();
	}

	public void Shoot(Node2D target)
	{
		if (!canShoot || !IsInstanceValid(target) || target == null || _upgradeComponent.BulletResource == null)
		{
			return;
		}

		SpawnBullet(target);
		canShoot = false;
	}

	private async void SpawnBullet(Node2D target)
	{
		var bullet = InstantiateBullet();

		bullet.Damage = _upgradeComponent.BulletResource.Damage;
		bullet.Speed = _upgradeComponent.BulletResource.Speed;
		
		bullet.Target = target;
		bullet.GlobalPosition = _bulletSpawnPoint.GlobalPosition;

		GD.Print($"Bullet fired with delay: {FireRateDelay} seconds");

		await ToSignal(GetTree().CreateTimer(FireRateDelay), "timeout");
		
		GD.Print("Ready to shoot again");
		canShoot = true;
	}

	private BaseBullet InstantiateBullet()
	{		
		var bullet = _upgradeComponent.BulletResource.Scene.Instantiate<BaseBullet>();
		GetTree().GetFirstNodeInGroup(nameof(Main)).AddChild(bullet);

		return bullet;
	}
}

