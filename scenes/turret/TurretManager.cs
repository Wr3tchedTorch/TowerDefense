using System.Linq;
using Game.Autoload;
using Game.Component;
using Godot;
using TurretDefense.enums;

namespace Game.Turret;

public partial class TurretManager : Node2D
{
	[ExportGroup("Attributes")]
	[Export]
	public TurretAttributesResource TurretAttributesResource
	{
		get => _turretAttributes;
		private set
		{
			_turretAttributes = value;
		}

	}
	[Export] private float buildingCooldown = .2f;

	[ExportCategory("Dependencies")]
	[Export] private ShootComponent ShootComponent { get; set; }
	[Export] private TurretAttributesComponent TurretAttributesComponent { get; set; }
	[Export] private TargetComponent TargetComponent { get; set; }
	[Export] private TurretFactory TurretFactory { get; set; }

	public bool IsBuilt = false;
	public Node2D BulletsGroup
	{
		get => bulletsGroup;
		set
		{
			bulletsGroup = value;
			if (CurrentTurret == null)
			{
				return;
			}
			ShootComponent.Initialize(
				TurretAttributesComponent,
				value,
				TurretAttributesComponent.TurretAttributesResource.FramePredictionAmount);
		}
	}

	private Node2D bulletsGroup = null;
	private BaseTurret CurrentTurret { get; set; } = null;

	private TurretAttributesResource _turretAttributes;
	private CollisionShape2D radiusCollisionShape = null;
	private TurretTier currentTier = TurretTier.TierOne;

	private Node2D currentTarget = null;

	public override void _Ready()
	{				
		TurretAttributesComponent.Initialize(TurretAttributesResource, Callable.From(UpdateTurret));

		ShootComponent.Initialize(
			TurretAttributesComponent,
			bulletsGroup,
			TurretAttributesComponent.TurretAttributesResource.FramePredictionAmount
		);

		TargetComponent.Initialize(TurretAttributesComponent, this);

		TurretFactory.Parent = this;
		TurretFactory.TurretSwitched += ShootComponent.OnTurretSwitched;
		TurretFactory.Initialize(
			ShootComponent,
			TurretAttributesComponent,
			TargetComponent,
			new Callable(this, "IsOutOfRange"),
			TurretAttributesComponent.TurretAttributesResource.FramePredictionAmount
		);
		CurrentTurret = TurretFactory.CurrentTurret;

		ShootComponent.BarrelMarkers = CurrentTurret.BarrelMarkers;
		ShootComponent.Shooting += CurrentTurret.OnShooting;

		UpdateTurret();
		StartBuildingCooldown();
	}

	public bool IsOutOfRange(Node2D enemy)
	{
		return GlobalPosition.DistanceTo(enemy.GlobalPosition) > TurretAttributesComponent.GetRadius();
	}

	private void UpdateTurret()
	{
		UpdateRadius();

		var toTier = TurretAttributesComponent.CurrentTurretAttributesResource.Tier;
		if (currentTier != toTier)
		{
			TurretFactory.SwitchTurrets((int)toTier);
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape = GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		if (radiusCollisionShape == null)
		{
			GD.PrintErr("BaseTurret (ln 82): No radius collision shape found.");
			return;
		}
		GD.Print($"BaseTurret (ln 85): {TurretAttributesComponent.Name}.");
		if (TurretAttributesComponent.CurrentTurretAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesComponent.GetRadius() };
	}

	public void OnMouseClick()
	{
		if (!IsBuilt)
		{
			return;
		}
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, TurretAttributesComponent, this);
	}

	private async void StartBuildingCooldown()
	{
		await ToSignal(GetTree().CreateTimer(buildingCooldown), "timeout");
		IsBuilt = true;
	}

	private float DistanceTo(Node2D enemy)
	{
		return GlobalPosition.DistanceTo(enemy.GlobalPosition);
	}
}
