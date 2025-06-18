using System.Linq;
using Game.Autoload;
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

	public CurrentTurretAttributesResource CurrentTurretAttributesResource { get; private set; } = null;

	public float Damage => _turretAttributes.Damage * (1 + CurrentTurretAttributesResource.DamageUpgradePercentage / 100f);
	public float FireRate =>
		Mathf.Clamp(
		60f / (
			_turretAttributes.FireRate
			* (1 + CurrentTurretAttributesResource.FireRateUpgradePercentage / 100f)
			* (1 + (int)CurrentTurretAttributesResource.Tier * 0.5f)
		),
		TurretAttributesResource.MinFireRateDelay,
		TurretAttributesResource.MaxFireRateDelay
	);
	public float BulletSpeed => _turretAttributes.BulletSpeed * (1 + CurrentTurretAttributesResource.BulletSpeedUpgradePercentage / 100f);
	public float Radius => _turretAttributes.Radius * (1 + CurrentTurretAttributesResource.RadiusUpgradePercentage / 100f);

	public BaseTurret CurrentTurret { get; private set; }

	[Export] private float buildingCooldown = .2f;
	public bool IsBuilt = false;

	private TurretAttributesResource _turretAttributes;

	private CollisionShape2D radiusCollisionShape = null;
	private TurretTier currentTier = TurretTier.TierOne;

	public override void _Ready()
	{
		CurrentTurretAttributesResource = new CurrentTurretAttributesResource();

		CurrentTurretAttributesResource.AttributesChanged += UpdateTurret;
		UpdateTurret();

		CurrentTurret = GetChildren().OfType<BaseTurret>().FirstOrDefault();
		CurrentTurret.IsBuilt = true;

		CurrentTurret.MouseClick += OnMouseClick;
		StartBuildingCooldown();
	}

	private void UpdateTurret()
	{
		UpdateRadius();

		if (currentTier !=CurrentTurretAttributesResource.Tier)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape ??= GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		
		if (CurrentTurretAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = Radius };
	}

	private void UpdateTurretScene()
	{
		CurrentTurret.QueueFree();
		CurrentTurret = null;

		currentTier = CurrentTurretAttributesResource.Tier;		
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
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, TurretAttributesResource, CurrentTurretAttributesResource);
	}

	private async void StartBuildingCooldown()
	{
		await ToSignal(GetTree().CreateTimer(buildingCooldown), "timeout");
		IsBuilt = true;
	}
}
