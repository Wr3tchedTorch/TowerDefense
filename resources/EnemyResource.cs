using Godot;

namespace Game.Resources;

public partial class EnemyResource : Node
{

	[Export] public int EnemySpawnDelay { get; set; }
	[Export] public int EnemyStartingWave { get; set; }
	[Export] public PackedScene EnemyScene { get; private set; }
}
