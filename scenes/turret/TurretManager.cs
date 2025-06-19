using System.Linq;
using Game.Autoload;
using Game.Component;
using Game.Extensions;
using Game.scripts.helper;
using Godot;
using TurretDefense.enums;

namespace Game.Turret;

[Tool]
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
			UpdateRadius();
		}

	}
	[Export] private float buildingCooldown = .2f;

	[ExportCategory("Dependencies")]
	[Export] private ShootComponent ShootComponent { get; set; }
	[Export] private TurretAttributesComponent TurretAttributesComponent { get; set; }
	[Export] private TargetComponent TargetComponent { get; set; }

	public bool IsBuilt = false;
	public Node2D BulletsGroup { get; set; } = null;

	private BaseTurret CurrentTurret { get; set; }

	private TurretAttributesResource _turretAttributes;
	private CollisionShape2D radiusCollisionShape = null;
	private TurretTier currentTier = TurretTier.TierOne;

	private Node2D currentTarget = null;

	public override void _Ready()
	{
		TurretAttributesComponent.Initialize(TurretAttributesResource, Callable.From(UpdateTurret));
		ShootComponent.Initialize(TurretAttributesComponent, BulletsGroup);
		TargetComponent.Initialize(TurretAttributesComponent, this);

		CurrentTurret = GetChildren().OfType<BaseTurret>().FirstOrDefault();
		CurrentTurret.MouseClick += OnMouseClick;

		UpdateTurret();
		StartBuildingCooldown();
	}

	public override void _Process(double delta)
	{
		var isTargetInvalid = currentTarget == null || DistanceTo(currentTarget) > TurretAttributesComponent.GetRadius();
		if (isTargetInvalid)
		{
			currentTarget = TargetComponent.GetTargetEnemy(new Callable(TargetComponent, TargetComponent.GetFirstEnemyInLineCallableName));
		}

		ShootComponent.SetTarget(currentTarget);		
	}

	private void UpdateTurret()
	{
		UpdateRadius();

		if (currentTier != TurretAttributesComponent.CurrentTurretAttributesResource.Tier)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape ??= GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");

		if (TurretAttributesComponent.CurrentTurretAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesComponent.GetRadius() };
	}

	private void UpdateTurretScene()
	{
		CurrentTurret.QueueFree();
		CurrentTurret = null;

		currentTier = TurretAttributesComponent.CurrentTurretAttributesResource.Tier;
		var turretScenePath = TurretAttributesResource.TurretTierScenes[(int)currentTier];

		CurrentTurret = InstantiateNewTurret(turretScenePath);
		AddChild(CurrentTurret);
	}

	private BaseTurret InstantiateNewTurret(string turretScenePath)
	{
		var scene = GD.Load<PackedScene>(turretScenePath);
		if (scene == null)
		{
			GD.PrintErr($"BaseTurret (ln 70): No turret scene found for tier {currentTier}.");
			return null;
		}
		var newTurret = scene.Instantiate<BaseTurret>();
		newTurret.MouseClick += OnMouseClick;
		return newTurret;
	}

	private void OnMouseClick()
	{
		if (!IsBuilt)
		{
			return;
		}
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, this, TurretAttributesComponent);
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
