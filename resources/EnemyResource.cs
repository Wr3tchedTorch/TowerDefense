using Godot;

namespace Game.Resources;

[GlobalClass]
public partial class EnemyResource : Resource
{
	[ExportGroup("Attributes")]
	[Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	[Export] public int TotalHealth { get; set; }
	[Export] public int Damage { get; set; }
	[Export] public int Speed { get; set; }

	[ExportGroup("Spawning Settings")]
	[Export] public int SpawnChance { get; set; }
	[Export] public int StartingWave { get; set; }
	[Export] public PackedScene Scene { get; private set; }
}
