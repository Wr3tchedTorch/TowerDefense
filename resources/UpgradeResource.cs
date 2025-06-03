using Godot;
using TowerDefense.enums;

[GlobalClass]
public partial class UpgradeResource : Resource
{
	[Export] public UpgradeType Type { get; set; }
	[Export] public int Power { get; set; }
	[Export] public int Cost { get; set; }	
}