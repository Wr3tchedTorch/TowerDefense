using Game.Autoload;
using Game.Enums;
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

	public CurrentTowerAttributesResource CurrentTowerAttributesResource { get;  private set; } = null;
	public Marker2D CenterMarker { get; private set; }

	public float Damage => _towerAttributes.Damage * (1 + CurrentTowerAttributesResource.DamageUpgradePercentage / 100f);
	public float FireRate => _towerAttributes.FireRate * (1 + CurrentTowerAttributesResource.FireRateUpgradePercentage / 100f);
	public float BulletSpeed => _towerAttributes.BulletSpeed * (1 + CurrentTowerAttributesResource.BulletSpeedUpgradePercentage / 100f);
	public float Radius => _towerAttributes.Radius * (1 + CurrentTowerAttributesResource.RadiusUpgradePercentage / 100f);

	private TowerAttributesResource _towerAttributes;
	private CollisionShape2D radiusCollisionShape;

	public override void _Ready()
	{
		CenterMarker = GetNode<Marker2D>("CenterMarker2D");
		CurrentTowerAttributesResource = new CurrentTowerAttributesResource();

		CurrentTowerAttributesResource.AttributesChanged += UpdateTower;
		UpdateTower();
	}

	private void UpdateTower()
	{
		UpdateRadius();

		UpdateSprite();
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

	private void UpdateSprite()
	{
		var sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		if (sprite == null)
		{
			GD.PrintErr("BaseTower (ln 70): No sprite found.");
			return;
		}
		sprite.Frame = (int) CurrentTowerAttributesResource.Tier;
	}

	private void OnMouseClick(Vector2 _)
	{
		GameEvents.Instance.EmitSignalOpenUpgradeMenu(TowerAttributesResource, CurrentTowerAttributesResource);
	}
}
