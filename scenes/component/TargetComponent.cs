using Game.Enemy;
using Game.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.scripts.helper;

namespace Game.Component;

public partial class TargetComponent : Node
{
	[Signal] public delegate void TargetChangedEventHandler(Node2D newTarget);
	[Signal] public delegate void NodeOutOfRangeEventHandler(Node2D node);

	public readonly string GetFirstEnemyInLineCallableName = nameof(GetFirstEnemyInLine);
	public readonly string GetLastEnemyInLineCallableName = nameof(GetLastEnemyInLine);
	public readonly string GetClosestEnemyInRadiusCallableName = nameof(GetClosestEnemyInRadius);
	public readonly string GetStrongestEnemyInRadiusCallableName = nameof(GetStrongestEnemyInRadius);
	public readonly string GetWeakestEnemyInRadiusCallableName = nameof(GetWeakestEnemyInRadius);

	private Node2D user;
	private Callable GetTargetCallable;
	private TurretAttributesComponent turretAttributesComponent;
	private readonly List<PathFollow2D> enemies = [];

	public void Initialize(TurretAttributesComponent turretAttributesComponent, Node2D user)
	{
		this.turretAttributesComponent = turretAttributesComponent;
		this.user = user;
	}

	public Node2D GetTargetEnemy(Callable getTargetCallable)
	{
		if (enemies.Count == 0)
		{
			return null;
		}

		if (enemies.Count == 1)
		{
			return enemies.FirstOrDefault();
		}

		return (Node2D)getTargetCallable.Call();
	}

	private Node2D GetWeakestEnemyInRadius()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) =>
			{
				if (firstEnemy is BaseEnemy fe && enemy is BaseEnemy e)
				{
					return fe.EnemyResource.TotalHealth <= e.EnemyResource.TotalHealth;
				}
				GD.PrintErr("TargetComponent: Not all units being compared are enemies");
				return false;
			}
		);
	}

	private Node2D GetStrongestEnemyInRadius()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) =>
			{
				if (firstEnemy is BaseEnemy fe && enemy is BaseEnemy e)
				{
					return fe.EnemyResource.TotalHealth >= e.EnemyResource.TotalHealth;
				}
				GD.PrintErr("TargetComponent: Not all units being compared are enemies");
				return false;
			}
		);
	}

	private Node2D GetClosestEnemyInRadius()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) => user.DistanceTo(firstEnemy) <= user.DistanceTo(enemy)
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
		enemies.RemoveAll(enemy => !IsInstanceValid(enemy));

		var firstEnemy = enemies.FirstOrDefault();
		if (firstEnemy == null)
		{
			return null;
		}

		foreach (var enemy in enemies)
		{
			if (enemy == null)
			{
				continue;
			}

			if (!isFirstEnemyBetter(firstEnemy, enemy))
			{
				firstEnemy = enemy;
			}
		}
		return firstEnemy;
	}

	private void OnAreaEntered(Area2D area)
	{
		var enemy = area.GetOwner<PathFollow2D>();
		if (enemies.Contains(enemy))
		{
			GD.PrintErr($"TargetComponent (ln 40): Enemy already tracked: {enemy.Name}");
			return;
		}
		enemies.Add(enemy);
	}

	private void OnAreaExited(Area2D area)
	{
		var enemy = area.GetOwner<PathFollow2D>();
		enemies.Remove(enemy);

		EmitSignal(SignalName.NodeOutOfRange, enemy);
	}
}
