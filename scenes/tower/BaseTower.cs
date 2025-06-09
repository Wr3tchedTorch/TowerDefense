using System.Linq;
using Game.Autoload;
using Godot;

namespace Game.Tower;

[Tool]
public partial class BaseTower : Node2D
{
	[ExportGroup("Attributes")]
	[Export]
	public TowerAttributesResource TowerAttributesResource
	{
		get => _towerAttributes;
		private set
		{
			_towerAttributes = value;
			UpdateTower();
		}
	}

	public CurrentTowerAttributesResource CurrentTowerAttributesResource { get; private set; } = null;
	public Marker2D CenterMarker { get; private set; }

	public float Damage => _towerAttributes.Damage * (1 + CurrentTowerAttributesResource.DamageUpgradePercentage / 100f);
	public float FireRate => _towerAttributes.FireRate * (1 + CurrentTowerAttributesResource.FireRateUpgradePercentage / 100f);
	public float BulletSpeed => _towerAttributes.BulletSpeed * (1 + CurrentTowerAttributesResource.BulletSpeedUpgradePercentage / 100f);
	public float Radius => _towerAttributes.Radius * (1 + CurrentTowerAttributesResource.RadiusUpgradePercentage / 100f);

	public BaseTurret CurrentTurret { get; private set; }

	private TowerAttributesResource _towerAttributes;
	private CollisionShape2D radiusCollisionShape;

	private Node2D target;
	private int currentTierIndex = 0;	

	public override void _Ready()
	{
		// CenterMarker = GetNode<Marker2D>("%CenterMarker2D");
		CurrentTowerAttributesResource = new CurrentTowerAttributesResource();

		CurrentTowerAttributesResource.AttributesChanged += UpdateTower;
		UpdateTower();

		CurrentTurret = GetChildren().OfType<BaseTurret>().FirstOrDefault();
		CurrentTurret.MouseClick += OnMouseClick;
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(target))
		{
			target = null;
			return;
		}
		LookAt(target.GlobalPosition);
	}

	private void UpdateTower()
	{
		UpdateRadius();

		if (CurrentTowerAttributesResource != null && (int)CurrentTowerAttributesResource.Tier != currentTierIndex)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape = GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		if (radiusCollisionShape == null)
		{
			GD.PrintErr("BaseTower (ln 60): No radius collision shape found.");
			return;
		}
		if (CurrentTowerAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TowerAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = Radius };
	}

	private void UpdateTurretScene()
	{
		CurrentTurret.QueueFree();
		CurrentTurret = null;

		currentTierIndex = (int)CurrentTowerAttributesResource.Tier;

		var scene = GD.Load<PackedScene>(TowerAttributesResource.TurretTierScenes[currentTierIndex]);
		if (scene == null)
		{
			GD.PrintErr($"BaseTower (ln 70): No turret scene found for tier {currentTierIndex}.");
			return;
		}
		CurrentTurret = scene.Instantiate<BaseTurret>();
		CurrentTurret.MouseClick += OnMouseClick;
		AddChild(CurrentTurret);
	}

	private void OnMouseClick()
	{
		GameEvents.Instance.EmitSignalOpenUpgradeMenu(TowerAttributesResource, CurrentTowerAttributesResource);
	}

	private void OnTargetChanged(Node2D newTarget)
	{
		target = newTarget;
	}
}
