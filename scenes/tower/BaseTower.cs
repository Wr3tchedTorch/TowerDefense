using System;
using System.Collections.Generic;
using System.Linq;
using Game.Autoload;
using Game.Component;
using Game.Enums;
using Godot;

namespace Game.Tower;

[Tool]
public partial class BaseTower : Node2D
{
	[Export] public TowerTargetMode TowerTargetMode { get; set; }
	[Export]
	public TowerAttributesResource TowerAttributesResource
	{
		get => _towerAttributes;
		private set
		{
			_towerAttributes = value;
			UpdateTowerRadius();
			UpdateTierSprites();
		}
	}

	public int CurrentTier { get; private set; }

	private TowerAttributesResource _towerAttributes;
	private CollisionShape2D _radiusCollisionShape;
	private Marker2D _centerMarker;

	public override void _Ready()
	{
		_centerMarker = GetNode<Marker2D>("CenterMarker2D");

		UpdateTowerRadius();
		UpdateTierSprites();
	}

	public override void _Process(double delta)
	{
		GD.Print(_towerAttributes.ToString());
    }

	public bool IsOutOfRange(float distance)
	{
		return distance > TowerAttributesResource.Radius;
	}

	public float GetDistanceToNode(Node2D target)
	{
		return (target.GlobalPosition - _centerMarker.GlobalPosition).Length();
	}

	public Node2D GetTargetEnemy()
	{
		switch (TowerTargetMode)
		{
			case TowerTargetMode.First:
				return GetFirstEnemyInLine();
			case TowerTargetMode.Last:
				return GetLastEnemyInLine();
			case TowerTargetMode.Closest:
				return GetClosestEnemyInRadius();
			default:
				GD.PrintErr("BaseTower (ln 30): No target mode selected, returning null.");
				throw new InvalidOperationException("No target mode selected.");
		}
	}

	private Node2D GetClosestEnemyInRadius()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) => GetDistanceToNode(firstEnemy) <= GetDistanceToNode(enemy)
		);
	}

	private Node2D GetFirstEnemyInLine()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) => firstEnemy.ProgressRatio >= enemy.ProgressRatio
		);
	}

	private Node2D GetLastEnemyInLine()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) => firstEnemy.ProgressRatio <= enemy.ProgressRatio
		);
	}

	private Node2D GetEnemyThroughCondition(Func<PathFollow2D, PathFollow2D, bool> isFirstEnemyBetter)
	{
		var enemies = GetAllEnemies();

		if (!enemies.Any())
		{
			return null;
		}

		var firstEnemy = enemies.First();
		foreach (var enemy in enemies)
		{
			if (IsOutOfRange(GetDistanceToNode(enemy)))
			{
				continue;
			}

			if (!isFirstEnemyBetter(firstEnemy, enemy))
			{
				firstEnemy = enemy;
			}
		}

		if (IsOutOfRange(GetDistanceToNode(firstEnemy)))
		{
			return null;
		}

		return firstEnemy;
	}

	private IEnumerable<PathFollow2D> GetAllEnemies()
	{
		return GetTree().GetNodesInGroup(nameof(Enemy)).Cast<PathFollow2D>();
	}

	private void UpdateTowerRadius()
	{
		_radiusCollisionShape = GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		if (_radiusCollisionShape == null)
		{
			GD.PrintErr("BaseTower (ln 60): No radius collision shape found.");
			return;
		}
		_radiusCollisionShape.Shape = new CircleShape2D() { Radius = TowerAttributesResource.Radius };
	}

	private void UpdateTierSprites()
	{

	}

	private void OnMouseClick(Vector2 position)
	{
		GameEvents.Instance.EmitSignalOpenUpgradeMenu(TowerAttributesResource);
		GD.Print("{Tower. ln 121}: clicked! opening upgrade menu.");
	}
}
