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

		var barrelCount = parent.CurrentTurret.BarrelMarkers.Length;
		var randomIndex =
			barrelCount == 1 ? 0 :
			GD.RandRange(0, barrelCount - 1);
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
		await ToSignal(GetTree().CreateTimer(parent.FireRate), "timeout");
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
