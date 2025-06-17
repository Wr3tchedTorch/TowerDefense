using Game.Enemy;
using Game.Resources;
using Godot;

public partial class EnemyManager : Node
{
	[Export] public EnemyResource[] EnemyResources { get; set; }

	private float spawnDelay = 0.5f; // Delay between enemy spawns

	public async void SpawnEnemies(int amount)
	{
		if (amount <= 0)
		{
			return;
		}
		var randomIndex = GD.RandRange(0, EnemyResources.Length - 1);
		SpawnEnemy(randomIndex);

		await ToSignal(GetTree().CreateTimer(spawnDelay), "timeout");
		SpawnEnemies(--amount);
	}

	private void SpawnEnemy(int index)
	{
		var enemy = EnemyResources[index].Scene.Instantiate<BaseEnemy>();
		enemy.Damage = EnemyResources[index].Damage;
		enemy.TotalHealth = EnemyResources[index].TotalHealth;

		var randomSpeed = GD.RandRange(0.75, 1.5);
		enemy.MovementSpeed = EnemyResources[index].Speed * (float)randomSpeed;
		GetTree().GetFirstNodeInGroup("Enemies").AddChild(enemy);
	}

	private void OnSpawnEnemyConsole(int amount)
	{
		if (amount <= 0)
		{
			GD.PrintErr("Amount must be greater than 0.");
			return;
		}
		SpawnEnemies(amount);
	}
}
