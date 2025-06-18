using System.Linq;
using Game.Autoload;
using Game.Component;
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
	public bool IsBuilt = false;

	public TurretAttributesCalculator TurretAttributesCalculator { get; private set; }
	public BaseTurret CurrentTurret { get; private set; }

	private TurretAttributesResource _turretAttributes;
	private CollisionShape2D radiusCollisionShape = null;
	private TurretTier currentTier = TurretTier.TierOne;

	public override void _Ready()
	{		
		var currentTurretAttributesResource = new CurrentTurretAttributesResource();
		TurretAttributesCalculator = new TurretAttributesCalculator(TurretAttributesResource, currentTurretAttributesResource);

		TurretAttributesCalculator.CurrentTurretAttributesResource.AttributesChanged += UpdateTurret;
		UpdateTurret();

		CurrentTurret = GetChildren().OfType<BaseTurret>().FirstOrDefault();
		CurrentTurret.IsBuilt = true;		

		CurrentTurret.MouseClick += OnMouseClick;
		StartBuildingCooldown();
	}

	private void UpdateTurret()
	{
		UpdateRadius();

		if (currentTier != TurretAttributesCalculator.CurrentTurretAttributesResource.Tier)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape ??= GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		
		if (TurretAttributesCalculator.CurrentTurretAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesCalculator.GetRadius() };
	}

	private void UpdateTurretScene()
	{
		CurrentTurret.QueueFree();
		CurrentTurret = null;

		currentTier = TurretAttributesCalculator.CurrentTurretAttributesResource.Tier;		
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
		newTurret.IsBuilt = true;
		return newTurret;
	}

	private void OnMouseClick()
	{
		if (!IsBuilt)
		{
			return;
		}
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, TurretAttributesResource, TurretAttributesCalculator.CurrentTurretAttributesResource);
	}

	private async void StartBuildingCooldown()
	{
		await ToSignal(GetTree().CreateTimer(buildingCooldown), "timeout");
		IsBuilt = true;
	}
}
