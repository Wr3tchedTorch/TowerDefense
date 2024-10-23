using Godot;

[GlobalClass]
public partial class TowerResource : Resource
{

	[Export] public int TowerRangeRadius { get; private set; }
	[Export] public int TowerCellWidth { get; private set; }
	[Export] public PackedScene TowerScene { get; private set; }
}
