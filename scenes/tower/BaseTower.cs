using System.Linq;
using Godot;

namespace Game.Tower;

[Tool]
public partial class BaseTower : Node2D
{

	[Export] public float Radius { get => _radius; set { _radius = value; UpdateTowerRadius(); } }

	private float _radius;
	private CollisionShape2D _radiusCollisionShape;
	private Marker2D _centerMarker;

	public override void _Ready()
	{

		_centerMarker = GetNode<Marker2D>("CenterMarker2D");
	}

	public Node2D GetClosestEnemyInRadius()
	{

		var enemies = GetTree().GetNodesInGroup(nameof(Enemy)).Cast<Node2D>();

		if (!enemies.Any())
			return null;

		Node2D closestEnemy = enemies.First();
		foreach (var enemy in enemies)
		{

			var enemyDistance = GetDistanceToNode(enemy);
			if (IsOutOfRange(enemyDistance))
				continue;

			if (enemyDistance < GetDistanceToNode(closestEnemy))
				closestEnemy = enemy;
		}
		if (IsOutOfRange(GetDistanceToNode(closestEnemy)))
			return null;

		return closestEnemy;
	}

	public bool IsOutOfRange(float distance)
	{
		return distance > _radius;
	}

	public float GetDistanceToNode(Node2D target)
	{
		return (target.GlobalPosition - _centerMarker.GlobalPosition).Length();
	}

	private void UpdateTowerRadius()
	{

		_radiusCollisionShape ??= GetNodeOrNull<CollisionShape2D>("RadiusArea2D/CollisionShape2D");
		if (!IsInstanceValid(_radiusCollisionShape))
			return;
		_radiusCollisionShape.Shape = new CircleShape2D() { Radius = Radius };
	}
}
