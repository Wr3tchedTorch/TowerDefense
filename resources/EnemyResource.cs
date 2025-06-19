using Godot;

namespace Game.Resources;

[GlobalClass]
public partial class EnemyResource : Resource
{
	[ExportGroup("Attributes")]
	[Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	
	[Export] public float TotalHealth { get; set; }
	[Export] public float Damage { get; set; }
	[Export] public float Speed { get; set; }

	[ExportGroup("Spawning Settings")]
	[Export] public int SpawnChance { get; set; }
	[Export] public int StartingWave { get; set; }
	[Export] public PackedScene Scene { get; private set; }
}
