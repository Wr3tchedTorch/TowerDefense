using System;
using Game.Bullet;
using Game.Extensions;
using Game.Turret;
using Godot;

public partial class ShootComponent : Node
{
	[Signal] public delegate void ShootingEventHandler();

	private TurretManager parent;
	private Node2D target = null;
	private bool canShoot = true;

	private float FireRateDelay
	{
		get
		{
			float percentage = Mathf.Clamp(parent.TurretAttributesResource.FireRate, 0f, 100f);
			float minDelay = parent.TurretAttributesResource.MinFireRateDelay;
			float maxDelay = parent.TurretAttributesResource.MaxFireRateDelay;

			float t = 1f - Mathf.Pow(percentage / 100f, 2f);
			return Mathf.Lerp(minDelay, maxDelay, t);
		}
	}

	public override void _Ready()
	{
		parent = GetOwner<TurretManager>();
	}

    public override void _Process(double delta)
    {
		if (!IsInstanceValid(target))
		{
			target = null;
			return;
		}
		Shoot(target);
    }

	public void Shoot(Node2D target)
	{
		if (!canShoot)
		{
			return;
		}

		var bullet = InstantiateBullet();
		if (bullet == null)
		{
			return;
		}
		EmitSignal(SignalName.Shooting);
		canShoot = false;

		var rand = new Random();
		var randomIndex = rand.Next(0, parent.CurrentTurret.BarrelMarkers.Length-1);
		bullet.GlobalPosition = parent.CurrentTurret.BarrelMarkers[randomIndex].GlobalPosition;
		
		bullet.Target = target;
		bullet.Damage = parent.Damage;
		bullet.Speed = parent.BulletSpeed;
		bullet.Penetration = parent.TurretAttributesResource.Penetration;
		GetTree().GetFirstNodeInGroup("Bullet").AddChild(bullet);

		ShootingCountdown();
	}

	private async void ShootingCountdown()
	{
		await ToSignal(GetTree().CreateTimer(FireRateDelay), "timeout");
		canShoot = true;
	}

	private BaseBullet InstantiateBullet()
	{
		var scene = GD.Load<PackedScene>(parent.TurretAttributesResource.BulletScenePath);
		var bullet = scene.Instantiate<BaseBullet>();
		if (bullet == null)
		{
			GD.PrintErr("ShootComponent (ln 10): Bullet scene is not set or could not be instantiated.");
			return null;
		}
		return bullet;
	}

	private void OnTargetChanged(Node2D target)
	{
		this.target = target;
	}
}
