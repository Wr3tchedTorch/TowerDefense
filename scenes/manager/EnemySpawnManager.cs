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
	private List<Timer> _enemiesSpawnTimers;
	private int _currentWave = 1;

	private bool _isWavePlaying = false;
	private bool _canStartWave = false;

	private EnemyResource[] EnemiesResourceInCurrentWave => _enemiesResource.Where(enemyResource => enemyResource.EnemyStartingWave == _currentWave).ToArray();


	public async override void _Ready()
	{
		await ToSignal(GetTree().CreateTimer(_delayBeforeStartingWave), "timeout");
		_canStartWave = true;
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
	}

	private void StartSpawningEnemies()
	{
		_isWavePlaying = true;

		foreach (var timer in _enemiesSpawnTimers)
		{
			timer.Start();
		}
	}

	private void ResetEnemiesTimers()
	{
		if (_enemiesSpawnTimers.Count > 0)
		{
			_enemiesSpawnTimers.ToList().ForEach(enemyTimer => enemyTimer.QueueFree());
			_enemiesSpawnTimers.Clear();
		}
		_enemiesSpawnTimers = CreateEnemiesSpawnTimer();
		_enemiesSpawnTimers.ForEach(enemyTimer => AddChild(enemyTimer));
	}

	private List<Timer> CreateEnemiesSpawnTimer()
	{
		List<Timer> timers = new();
		foreach (var enemyResource in EnemiesResourceInCurrentWave)
		{

			Timer enemyTimer = new()
			{
				WaitTime = enemyResource.EnemySpawnDelay,
				OneShot = false
			};
			enemyTimer.Timeout += () =>
			{
				var enemy = enemyResource.EnemyScene.Instantiate<PathFollow2D>();
				_enemiesPath.AddChild(enemy);
				enemy.Progress = 0;
			};
			timers.Add(enemyTimer);
		}
		return timers.ToList();
	}
}
