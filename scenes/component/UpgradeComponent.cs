using System.Collections.Generic;
using Godot;
using TowerDefense.enums;

public partial class UpgradeComponent : Node
{
	[Signal] public delegate void IncreaseUnitCountEventHandler(int amount);

	[Export] public BulletResource BulletResource { get; private set; }

	[Export] private UpgradePathResource upgradePaths;

	private bool isOpen = false;

	private readonly Dictionary<UpgradePath, int> upgradeStateIndex = new(3)
	{
		{ UpgradePath.Top,    0 },
		{ UpgradePath.Middle, 0 },
		{ UpgradePath.Bottom, 0 }
	};

	private Dictionary<UpgradePath, Godot.Collections.Array<UpgradeResource>> upgradePathIndex;

	public override void _Ready()
	{
		upgradePathIndex = new(3)
		{
			{ UpgradePath.Top,    upgradePaths.TopPath },
			{ UpgradePath.Middle, upgradePaths.MiddlePath },
			{ UpgradePath.Bottom, upgradePaths.BottomPath }
		};
	}

	public void Upgrade(UpgradePath path)
	{
		if (upgradeStateIndex[path] >= upgradePathIndex[path].Count)
		{
			GD.PrintErr($"No more upgrades available for path: {path}");
			return;
		}
	}
}
