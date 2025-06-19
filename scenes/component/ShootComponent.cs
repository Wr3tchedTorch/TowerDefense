using Game.Bullet;
using Game.Component;
using Godot;

public partial class ShootComponent : Node
{
	[Signal] public delegate void ShootingEventHandler();

	public Marker2D[] BarrelMarkers;
	public Node2D BulletsGroup { get; set; } = null;

	private TurretAttributesComponent turretAttributesComponent;
	private Node2D target = null;
	private bool canShoot = true;	
	
	public void Initialize(TurretAttributesComponent turretAttributesComponent, Node2D bulletsGroup)
	{
		if (bulletsGroup == null)
		{
			GD.PrintErr("ShootComponent (ln 6): BulletsGroup is null.");
		}

		this.turretAttributesComponent = turretAttributesComponent;
		BulletsGroup = bulletsGroup;
	}

	public void Initialize(TurretAttributesComponent turretAttributesComponent, Node2D bulletsGroup, Callable OnShooting)
	{
		if (bulletsGroup == null)
		{
			GD.PrintErr("ShootComponent (ln 6): BulletsGroup is null.");
		}

		this.turretAttributesComponent = turretAttributesComponent;
		BulletsGroup = bulletsGroup;

		Connect(SignalName.Shooting, OnShooting);
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
		canShoot = false;
		EmitSignal(SignalName.Shooting);

		var barrels = BarrelMarkers;
		foreach (var barrel in barrels)
		{
			var bullet = InstantiateBullet(barrel);

			GetTree().GetFirstNodeInGroup("Bullets").AddChild(bullet);
		}
		ShootingCountdown();
	}

	public void SetTarget(Node2D target)
	{
		if (target == null || !IsInstanceValid(target))
		{
			GD.PrintErr("ShootComponent (ln 34): Target is null or invalid.");
			return;
		}
		this.target = target;
	}

	private async void ShootingCountdown()
	{
		await ToSignal(GetTree().CreateTimer(turretAttributesComponent.GetFireRate()), "timeout");
		canShoot = true;
	}

	private BaseBullet InstantiateBullet(Marker2D barrel)
	{
		var scene = GD.Load<PackedScene>(turretAttributesComponent.TurretAttributesResource.BulletScenePath);
		var bullet = scene.Instantiate<BaseBullet>();
		if (bullet == null)
		{
			GD.PrintErr("ShootComponent (ln 10): Bullet scene is not set or could not be instantiated.");
			return null;
		}					
		bullet.GlobalPosition = barrel.GlobalPosition;
		bullet.Target 	   = target;
		bullet.Damage 	   = turretAttributesComponent.GetDamage();
		bullet.Speed 	   = turretAttributesComponent.GetBulletSpeed();
		bullet.Penetration = turretAttributesComponent.TurretAttributesResource.Penetration;
		return bullet;
	}
}
