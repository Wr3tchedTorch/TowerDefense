using Godot;

namespace Game.Component;

public partial class HealthComponent : Node
{

	[Signal] public delegate void DeathEventHandler();

	[Export] private ProgressBar _progressBar;

	public float MaxHealth { get; set; }

	public float CurrentHealth
	{
		get { return _currentHealth; }
		private set { _currentHealth = value; _progressBar.Value = value; }
	}

	private float _currentHealth;

	public void SetMaxHealth(float toMaxHealth)
	{
		MaxHealth = toMaxHealth;
		CurrentHealth = toMaxHealth;
		_progressBar.MaxValue = toMaxHealth;
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
		{
			EmitSignal(SignalName.Death);
		}
	}

	public void Heal(float healCount)
	{
		if (healCount < 0)
		{
			GD.Print($"{nameof(HealthComponent)} can't take negative heal. Try using Damage() instead.");
			return;
		}

		CurrentHealth = Mathf.Min(CurrentHealth + healCount, MaxHealth);
	}
}
