using System.Linq;
using Game.Unit;
using Godot;

namespace Game.Component;

public partial class RangedAttackComponent : Node
{

	[Export] private Marker2D[] _bulletSpawnPoints;

	// Make these attributes into a bullet resource
	[Export(PropertyHint.Range, "0,100")] private float _fireRatePercentage;
	[Export] private PackedScene _bulletScene;
	[Export] private float _bulletDamage;

	private RangedUnit _parent;
	private bool _canSpawnBullet = true;

	private float FireRateDelay => 1.8f - (1.5f * (_fireRatePercentage / 100));
	private Node2D Target => _parent.Target;

	public override void _Ready()
	{

		_parent = GetParent<RangedUnit>();
	}

	public override void _Process(double delta)
	{

		if (!_canSpawnBullet || !IsInstanceValid(Target) || Target == null)
			return;

		SpawnBullet();
		_canSpawnBullet = false;
	}

	private async void SpawnBullet()
	{

		GD.Print($"SpawnBullet - targeting: {Target.Name} at {Target.GlobalPosition}");

		await ToSignal(GetTree().CreateTimer(FireRateDelay), "timeout");
		_canSpawnBullet = true;
	}
}

