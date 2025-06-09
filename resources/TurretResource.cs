using Godot;

[GlobalClass]
public partial class TurretResource : Resource
{
	[Export(PropertyHint.File, "*.tscn,")]
	public string TurretScenePath { get; private set; }
	[Export] public int TurretCellWidth { get; private set; }
}
