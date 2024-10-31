using System.Collections.Generic;
using System.Linq;
using Game.Resources;
using Godot;

namespace Game.Manager;

public partial class EnemySpawnManager : Node
{

	[Export] private int _delayBeforeStartingWave;
	[Export] private int _numberOfWaves;
	[Export] private EnemyResource[] _enemiesResource;
	[Export] private Path2D _enemiesPath;

	private EnemyResource[] _currentEnemiesInThisWave;
	private Timer[] _enemiesSpawnTimers;
	private int _currentWave = 1;
	
	private bool _isWavePlaying = false;
	private bool _canStartWave = false;
	// private bool _i = false;

	private EnemyResource[] EnemiesResourceInCurrentWave => _enemiesResource.Where(enemyResource => enemyResource.EnemyStartingWave == _currentWave).ToArray();


	public async override void _Ready()
	{

		await ToSignal(GetTree().CreateTimer(_delayBeforeStartingWave), "timeout");
		_isWavePlaying = true;
	}

	public override void _Process(double delta)
	{

		if (_isWavePlaying || !_canStartWave)
			return;

		StartSpawningEnemies();
	}

	private async void GoToNextWave()
	{

		_canStartWave = false;
		_isWavePlaying = false;

		ResetEnemiesTimers();
		_currentWave++;

		await ToSignal(GetTree().CreateTimer(_delayBeforeStartingWave), "timeout");
		_canStartWave = true;
		_isWavePlaying = true;
	}

	private void StartSpawningEnemies()
	{

		_isWavePlaying = true;

		foreach (var enemyResource in EnemiesResourceInCurrentWave)
		{
			GetTree().CreateTimer(enemyResource.EnemySpawnDelay).Timeout += () =>
			{
				GD.Print("bunda");
				var enemy = enemyResource.EnemyScene.Instantiate<PathFollow2D>();
				_enemiesPath.AddChild(enemy);
				enemy.Progress = 0;
			};
		}
	}

	private void ResetEnemiesTimers()
	{
		if (_enemiesSpawnTimers.Length > 0)
			_enemiesSpawnTimers.ToList().ForEach(enemyTimer => enemyTimer.QueueFree());
		_enemiesSpawnTimers = CreateEnemiesSpawnTimer();
		_enemiesSpawnTimers.ToList().ForEach(enemyTimer => AddChild(enemyTimer));
	}

	private Timer[] CreateEnemiesSpawnTimer()
	{

		List<Timer> timers = new();
		foreach (var enemyResource in EnemiesResourceInCurrentWave)
		{

			Timer enemyTimer = new()
			{
				WaitTime = enemyResource.EnemySpawnDelay,
				OneShot = false
			};
			timers.Add(enemyTimer);
		}
		return timers.ToArray();
	}
}
