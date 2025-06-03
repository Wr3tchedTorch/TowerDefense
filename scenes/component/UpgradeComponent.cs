using Godot;
using TowerDefense.enums;

public partial class UpgradeComponent : Node
{
	public BulletResource BulletResource {get; private set;}	

	[Export(PropertyHint.File, "*.tres")] private string _bulletResourceFilePath;

	// Upgrade type and amount
	[Export] private Godot.Collections.Dictionary<UpgradeType, int> upgrades;

    public override void _Ready()
    {
		BulletResource = GD.Load<BulletResource>(_bulletResourceFilePath);
	}
}
