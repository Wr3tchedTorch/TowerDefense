using Godot;

[GlobalClass]
public partial class TowerResource : Resource
{
	[Export(PropertyHint.File, "*.tscn,")]
	public string TowerScenePath { get; private set; }
	[Export] public int TowerCellWidth { get; private set; }
}
