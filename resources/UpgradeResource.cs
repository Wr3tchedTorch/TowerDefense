using Godot;
using TowerDefense.enums;

[GlobalClass]
public partial class UpgradeResource : Node
{
	public UpgradeType Type { get; set; }
	public int Amount { get; set; }
}
