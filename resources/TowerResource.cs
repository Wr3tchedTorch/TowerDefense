using Godot;

[GlobalClass]
public partial class TowerResource : Resource
{
	[Export] public int TowerCellWidth { get; private set; }
	[Export] public PackedScene TowerScene { get; private set; }
	[Export] public string TowerName { get; private set; }
	[Export] public int CurrentTier { get; private set; }
}
