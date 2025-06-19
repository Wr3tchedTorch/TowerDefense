using Godot;

namespace Game.Component;

public partial class HealthComponent : Node
{

	[Signal] public delegate void DeathEventHandler();

	public ProgressBar HealthBar = null;
	public float MaxHealth { get; set; }

	public float CurrentHealth
	{
		get { return _currentHealth; }
		private set
		{
			_currentHealth = value;
			HealthBar.Value = value;
		}
	}

	private float _currentHealth;

	public void Initialize(float maxHealth)
	{
		MaxHealth = maxHealth;
		CurrentHealth = maxHealth;
	}

	public void Initialize(float maxHealth, ProgressBar healthBar)
	{
		HealthBar = healthBar;

		MaxHealth = maxHealth;
		CurrentHealth = maxHealth;

		if (HealthBar == null)
		{
			GD.Print($"{nameof(HealthComponent)}: HealthBar is not assigned. Please assign a ProgressBar node.");
			return;
		}
		HealthBar.MaxValue = maxHealth;
		HealthBar.Value = CurrentHealth;
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
