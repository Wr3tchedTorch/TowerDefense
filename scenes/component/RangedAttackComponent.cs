using Game.Bullet;
using Game.Unit;
using Godot;

namespace Game.Component;

public partial class RangedAttackComponent : Node
{
	[Export] private Marker2D _bulletSpawnPoint;

	[Export(PropertyHint.File, "*.tres")] private string _bulletResourceFilePath;

	private RangedUnit parent;
	private BulletResource bulletResource = null;
	private bool canShoot = true;

	private float FireRateDelay
	{
		get
		{
			float percentage = Mathf.Clamp(bulletResource.FireRatePercentage, 0f, 100f);
			float minDelay = bulletResource.MinFireRateDelay;
			float maxDelay = bulletResource.MaxFireRateDelay;
			
			float t = 1f - Mathf.Pow(percentage / 100f, 2f);
			return Mathf.Lerp(minDelay, maxDelay, t);
		}
	}

	public override void _Ready()
	{
		bulletResource = GD.Load<BulletResource>(_bulletResourceFilePath);

		parent = GetParent<RangedUnit>();
	}

	public void Shoot(Node2D target)
	{
		if (!canShoot || !IsInstanceValid(target) || target == null || bulletResource == null)
		{
			return;
		}

		SpawnBullet(target);
		canShoot = false;
	}

	private async void SpawnBullet(Node2D target)
	{
		var bullet = InstantiateBullet();

		bullet.damage = bulletResource.Damage;
		bullet.speed = bulletResource.Speed;
		
		bullet.Target = target;
		bullet.GlobalPosition = _bulletSpawnPoint.GlobalPosition;

		await ToSignal(GetTree().CreateTimer(FireRateDelay), "timeout");
		canShoot = true;
	}

	private BaseBullet InstantiateBullet()
	{		
		var bullet = bulletResource.Scene.Instantiate<BaseBullet>();
		GetTree().GetFirstNodeInGroup(nameof(Main)).AddChild(bullet);

		return bullet;
	}
}

