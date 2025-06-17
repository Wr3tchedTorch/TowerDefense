using System.Linq;
using Game.Autoload;
using Godot;

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
			UpdateTurret();
		}
	}

	public CurrentTurretAttributesResource CurrentTurretAttributesResource { get; private set; } = null;
	public Marker2D CenterMarker { get; private set; }

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
	private CollisionShape2D radiusCollisionShape;

	private Node2D target;
	private int currentTierIndex = 0;

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

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(target) || !IsBuilt)
		{
			target = null;
			return;
		}
		LookAt(target.GlobalPosition);
	}

	private void UpdateTurret()
	{
		UpdateRadius();

		if (CurrentTurretAttributesResource != null && (int)CurrentTurretAttributesResource.Tier != currentTierIndex)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape = GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		if (radiusCollisionShape == null)
		{
			GD.PrintErr("BaseTurret (ln 60): No radius collision shape found.");
			return;
		}
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

		currentTierIndex = (int)CurrentTurretAttributesResource.Tier;

		var scene = GD.Load<PackedScene>(TurretAttributesResource.TurretTierScenes[currentTierIndex]);
		if (scene == null)
		{
			GD.PrintErr($"BaseTurret (ln 70): No turret scene found for tier {currentTierIndex}.");
			return;
		}
		CurrentTurret = scene.Instantiate<BaseTurret>();
		CurrentTurret.MouseClick += OnMouseClick;
		CurrentTurret.IsBuilt = true;
		AddChild(CurrentTurret);
	}

	private void OnMouseClick()
	{
		if (!IsBuilt)
		{
			return;
		}
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, TurretAttributesResource, CurrentTurretAttributesResource);
	}

	private void OnTargetChanged(Node2D newTarget)
	{
		target = newTarget;
	}

	private async void StartBuildingCooldown()
	{
		await ToSignal(GetTree().CreateTimer(buildingCooldown), "timeout");
		IsBuilt = true;
	}
}
