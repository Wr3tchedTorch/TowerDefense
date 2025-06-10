using Game.Resources;
using Godot;

public partial class EnemyManager : Node
{
	[Export] public EnemyResource[] EnemyResources { get; set; }

	public override void _Ready()
	{
	}

	public void SpawnEnemies(int amount)
	{
		SpawnEnemy(EnemyResources[0].EnemyScene);
	}

	private void SpawnEnemy(PackedScene enemyScene)
	{
		var enemyInstance = enemyScene.Instantiate();
		AddChild(enemyInstance);
		// Initialize enemy instance if needed
	}
}
