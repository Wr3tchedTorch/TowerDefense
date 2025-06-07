using Godot;

namespace Game.Component;

public partial class HealthComponent : Node
{

	[Signal] public delegate void DeathEventHandler();

	[Export] private float _maxHealth;
	[Export] private ProgressBar _progressBar;

	private float _currentHealth;

	public float CurrentHealth
	{
		get { return _currentHealth; }
		private set { _currentHealth = value; _progressBar.Value = value; }
	}

	public override void _Ready()
	{

		CurrentHealth = _maxHealth;
		_progressBar.MaxValue = _maxHealth;
		_progressBar.Value = CurrentHealth;
	}

	public void Damage(float damageCount)
	{
		if (damageCount < 0)
		{
			GD.Print($"{nameof(HealthComponent)} can't take negative damage. Try using Heal() instead.");
			return;
		}

		CurrentHealth = Mathf.Max(CurrentHealth - damageCount, 0);

		if (CurrentHealth == 0)
			EmitSignal(SignalName.Death);
	}

	public void Heal(float healCount)
	{
		if (healCount < 0)
		{
			GD.Print($"{nameof(HealthComponent)} can't take negative heal. Try using Damage() instead.");
			return;
		}

		CurrentHealth = Mathf.Min(CurrentHealth + healCount, _maxHealth);
	}
}
