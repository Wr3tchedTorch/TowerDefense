using Game.Enums;
using Game.Extensions;
using Game.Turret;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetComponent : Node
{
	[Signal] public delegate void TargetChangedEventHandler(Node2D newTarget);

	public Node2D Target { get; private set; } = null;

	private TurretManager parent;

	private readonly List<PathFollow2D> enemies = new();

	public override void _Ready()
	{
		parent = GetOwner<TurretManager>();
	}

	public Node2D GetTargetEnemy()
	{
		switch (parent.CurrentTurretAttributesResource.TurretTargetMode)
		{
			case TurretTargetMode.First:
				return GetFirstEnemyInLine();
			case TurretTargetMode.Last:
				return GetLastEnemyInLine();
			case TurretTargetMode.Closest:
				return GetClosestEnemyInRadius();
			default:
				GD.PrintErr("BaseTurret (ln 30): No target mode selected, returning null.");
				throw new InvalidOperationException("No target mode selected.");
		}
	}

	private Node2D GetClosestEnemyInRadius()
	{
		return GetEnemyThroughCondition(
			(firstEnemy, enemy) => parent.GetDistanceToNode(firstEnemy) <= parent.GetDistanceToNode(enemy)
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
		if (!enemies.Any())
		{
			return null;
		}
		var firstEnemy = enemies.First();
		foreach (var enemy in enemies)
		{
			if (!isFirstEnemyBetter(firstEnemy, enemy))
			{
				firstEnemy = enemy;
			}
		}
		if (!IsInstanceValid(firstEnemy))
		{
			return null;
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
		var previousTarget = Target;

		Target = GetTargetEnemy();
		if (Target == null || Target != previousTarget)
		{
			EmitSignal(SignalName.TargetChanged, Target);
		}
	}

	private void OnAreaExited(Area2D area)
	{
		var enemy = area.GetOwner<PathFollow2D>();
		if (Target == enemy)
		{
			Target = null;
			EmitSignal(SignalName.TargetChanged, Target);
		}
		enemies.Remove(enemy);
	}
}
