using Game.Bullet;
using Game.Component;
using Game.Turret;
using Godot;

public partial class ShootComponent : Node
{
	[Signal] public delegate void ShootingEventHandler(Node2D target);

	public Marker2D[] BarrelMarkers;
	public Node2D BulletsGroup { get; set; } = null;
	public bool IsShooting { get; private set; } = false;

	private TurretAttributesComponent turretAttributesComponent;
	private Node2D target = null;
	private bool canShoot = true;
	private int framePredictionAmount;

	public void Initialize(
		TurretAttributesComponent turretAttributesComponent,
		Node2D bulletsGroup,
		int framePredictionAmount
	)
	{
		if (bulletsGroup == null)
		{
			GD.PrintErr("ShootComponent: BulletsGroup is null.");
		}
		this.turretAttributesComponent = turretAttributesComponent;
		this.framePredictionAmount = framePredictionAmount;
		BulletsGroup = bulletsGroup;
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(target) || target == null)
		{
			return;
		}
		Shoot(target);
	}

	public void Shoot(Node2D target)
	{
		if (!canShoot || !IsShooting)
		{
			return;
		}
		canShoot = false;
		EmitSignal(SignalName.Shooting, target);

		var barrels = BarrelMarkers;
		foreach (var barrel in barrels)
		{
			var bullet = InstantiateBullet(barrel);

			BulletsGroup.AddChild(bullet);
		}
		ShootingCountdown();
	}

	public void SetTarget(Node2D target)
	{
		if (target == null || !IsInstanceValid(target))
		{
			return;
		}
		this.target = target;
	}

	public void StartShooting()
	{
		if (IsShooting)
		{
			return;
		}
		IsShooting = true;
	}

	public void StopShooting()
	{
		if (!IsShooting)
		{
			return;
		}
		IsShooting = false;
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
			GD.PrintErr("ShootComponent: Bullet scene is not set or could not be instantiated.");
			return null;
		}
		bullet.GlobalPosition = barrel.GlobalPosition;
		bullet.Target = target;
		bullet.Damage = turretAttributesComponent.GetDamage();
		bullet.Speed = turretAttributesComponent.GetBulletSpeed();
		bullet.Penetration = turretAttributesComponent.TurretAttributesResource.Penetration;
		bullet.MaxMovementDistance = turretAttributesComponent.GetRadius();
		bullet.TurretPosition = GetOwner<Node2D>().GlobalPosition;

		bullet.Init(framePredictionAmount);
		return bullet;
	}

	public void OnTurretSwitched(BaseTurret newTurret)
	{	
		Shooting += newTurret.OnShooting;
		BarrelMarkers = newTurret.BarrelMarkers;
	}
}
