using Godot;

namespace Game.Tower;

[Tool]
public partial class Tower : Node2D
{

	[Export] public float Radius { get => _radius; set { _radius = value; UpdateTowerRadius(); } }

	private float _radius;

	private CollisionShape2D _radiusCollisionShape;
	private Area2D _radiusArea;

	public override void _Ready()
	{

		_radiusCollisionShape = GetNode<CollisionShape2D>("RadiusArea2D/CollisionShape2D");
		_radiusArea = GetNode<Area2D>("RadiusArea2D");

		_radiusArea.AreaEntered += OnRadiusArea2DAreaEntered;
	}

	private void UpdateTowerRadius()
	{

		if (!IsInstanceValid(_radiusCollisionShape))
			_radiusCollisionShape = GetNode<CollisionShape2D>("RadiusArea2D/CollisionShape2D");

		_radiusCollisionShape.Shape = new CircleShape2D() { Radius = Radius };
	}

	private void OnRadiusArea2DAreaEntered(Area2D area)
	{

		GD.Print($"{area.Name} is in this tower's range!");
	}
}
