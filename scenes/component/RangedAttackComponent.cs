using System.Linq;
using Godot;

namespace Game.Component;

public partial class RangedAttackComponent : Node
{

	[Export(PropertyHint.Range, "0,100")] private float _fireRatePercentage;
	[Export] private Marker2D[] _bulletSpawnPoints;
	[Export] private PackedScene _bulletScene;
	[Export] private float _bulletDamage;

	private CharacterBody2D _targetEnemy;
	private Node2D _parent;

	private float FireRateDelay => 1.8f - (1.5f * (_fireRatePercentage / 100));

	public override void _Ready()
	{

		_parent = GetParent<Node2D>();
	}

	public async override void _Process(double delta)
	{
		if (!IsInstanceValid(_targetEnemy))
			return;

		await ToSignal(GetTree().CreateTimer(FireRateDelay), "timeout");

		CharacterBody2D closestEnemy = GetClosestEnemyInRadius();
		if (closestEnemy == null)
			return;

		SpawnBullet(closestEnemy);
	}

	private CharacterBody2D GetClosestEnemyInRadius()
	{
		CharacterBody2D[] allEnemies = GetTree().GetNodesInGroup(nameof(Enemy)).Cast<CharacterBody2D>().ToArray();
		if (allEnemies.Length == 0)
			return null;
		if (allEnemies.Length == 1)
			return allEnemies[0];

		CharacterBody2D closestEnemy = allEnemies[0];
		foreach (var enemy in allEnemies)
		{
			if (GetDistanceToParent(enemy) < GetDistanceToParent(closestEnemy))
				closestEnemy = enemy;
		}

		return closestEnemy;
	}

	private float GetDistanceToParent(CharacterBody2D target)
	{
		return (target.GlobalPosition - _parent.GlobalPosition).Length();
	}

	private void SpawnBullet(CharacterBody2D target)
	{
		GD.Print($"SpawnBullet - targeting: {target}");
	}
}

