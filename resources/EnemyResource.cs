using Godot;

namespace Game.Resources;

public partial class EnemyResource : Resource
{
	[ExportGroup("Attributes")]
	[Export] public string EnemyName { get; set; }
	[Export] public string EnemyDescription { get; set; }
	[Export] public int EnemyHealth { get; set; }
	[Export] public int EnemyDamage { get; set; }
	[Export] public int EnemySpeed { get; set; }
	
	[ExportGroup("Spawning Settings")]
	[Export] public int EnemySpawnChance { get; set; }
	[Export] public int EnemySpawnDelay { get; set; }
	[Export] public int EnemyStartingWave { get; set; }
	[Export] public PackedScene EnemyScene { get; private set; }
}
