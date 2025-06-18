using Godot;

namespace Game.Manager;

public partial class EnemyManager : Node
{
	private readonly float spawnDelay = 0.5f;

	public Path2D[] EnemyGroups = [];
	public EnemyFactory EnemyFactory = null;

	public override void _Ready()
	{
		if (EnemyFactory == null || EnemyGroups.Length == 0)
		{
			GD.PrintErr("{EnemyManager ln.18}: EnemyFactory and EnemyGroups must be assigned before use.");
		}
	}

	public async void SpawnEnemies(int amount)
	{
		if (amount <= 0)
		{
			return;
		}
		var randomIndex = GD.RandRange(0, EnemyFactory.EnemyResources.Length - 1);
		SpawnEnemy(randomIndex);

		await ToSignal(GetTree().CreateTimer(spawnDelay), "timeout");
		SpawnEnemies(--amount);
	}

	private void SpawnEnemy(int index)
	{
		var enemy = EnemyFactory.CreateEnemy(index);
		var randomGroup = GD.RandRange(0, EnemyGroups.Length-1);
		EnemyGroups[randomGroup].AddChild(enemy);
	}

	private void OnSpawnEnemyRequested(int amount)
	{
		if (amount <= 0)
		{
			GD.PrintErr("Amount must be greater than 0.");
			return;
		}
		SpawnEnemies(amount);
	}
}
