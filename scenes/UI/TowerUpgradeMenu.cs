using Game.Autoload;
using Game.Enums;
using Godot;

public partial class TowerUpgradeMenu : Control
{
	[Signal] public delegate void UpgradeEventHandler(double cost);

	private bool isOpen = false;

	private Label NameLabel;
	private Label DescriptionLabel;
	private Label CurrentTierLabel;
	private ProgressBar damageProgressBar;
	private ProgressBar fireRateProgressBar;
	private ProgressBar bulletSpeedProgressBar;
	private ProgressBar radiusProgressBar;
	private OptionButton targetModeOptionButton;

	private TowerAttributesResource towerAttributesResource;

	public override void _Ready()
	{
		GameEvents.Instance.OpenUpgradeMenu += OnOpenUpgradeMenu;

		NameLabel = GetNode<Label>($"%{nameof(NameLabel)}");
		DescriptionLabel = GetNode<Label>($"%{nameof(DescriptionLabel)}");
		CurrentTierLabel = GetNode<Label>($"%{nameof(CurrentTierLabel)}");
		damageProgressBar = GetNode<ProgressBar>("%DamageProgressBar");
		fireRateProgressBar = GetNode<ProgressBar>("%FireRateProgressBar");
		bulletSpeedProgressBar = GetNode<ProgressBar>("%BulletSpeedProgressBar");
		radiusProgressBar = GetNode<ProgressBar>("%RangeProgressBar");
		targetModeOptionButton = GetNode<OptionButton>("%TargetModeOptionButton");

		targetModeOptionButton.AddItem(TowerTargetMode.First.ToString(), (int)TowerTargetMode.First);
		targetModeOptionButton.AddItem(TowerTargetMode.Last.ToString(), (int)TowerTargetMode.Last);
		targetModeOptionButton.AddItem(TowerTargetMode.Closest.ToString(), (int)TowerTargetMode.Closest);
	}

	private void OnOpenUpgradeMenu(TowerAttributesResource towerAttributesResource)
	{
		if (isOpen)
		{
			GD.PrintErr("UpgradeComponent is already open.");
			return;
		}
		this.towerAttributesResource = towerAttributesResource;
		UpdateInfo();
		Visible = true;
		isOpen = true;
	}

	private void OnCloseButtonPressed()
	{
		Visible = false;
		isOpen = false;
	}

	private void UpdateInfo()
	{
		NameLabel.Text = towerAttributesResource.Name;
		DescriptionLabel.Text = towerAttributesResource.Description;
		CurrentTierLabel.Text = $"Current Tier: {towerAttributesResource.CurrentTier}";

		damageProgressBar.Value = towerAttributesResource.CurrentDamageUpgradePercentage;
		fireRateProgressBar.Value = towerAttributesResource.CurrentFireRateUpgradePercentage;
		bulletSpeedProgressBar.Value = towerAttributesResource.CurrentBulletSpeedUpgradePercentage;
		radiusProgressBar.Value = towerAttributesResource.CurrentRadiusUpgradePercentage;

		targetModeOptionButton.Select((int)towerAttributesResource.TowerTargetMode);
	}

	private void OnDamageUpgradeButtonPressed()
	{
		if (towerAttributesResource.CurrentDamageUpgradePercentage == 100)
		{
			GD.Print("Maximum damage upgrade reached.");
			return;
		}
		towerAttributesResource.CurrentDamageUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnFireRateUpgradeButtonPressed()
	{
		if (towerAttributesResource.CurrentFireRateUpgradePercentage == 100)
		{
			GD.Print("Maximum fire rate upgrade reached.");
			return;
		}
		towerAttributesResource.CurrentFireRateUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnBulletSpeedUpgradeButtonPressed()
	{
		if (towerAttributesResource.CurrentBulletSpeedUpgradePercentage == 100)
		{
			GD.Print("Maximum bullet speed upgrade reached.");
			return;
		}
		towerAttributesResource.CurrentBulletSpeedUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnRangeUpgradeButtonPressed()
	{
		if (towerAttributesResource.CurrentRadiusUpgradePercentage == 100)
		{
			GD.Print("Maximum range upgrade reached.");
			return;
		}
		towerAttributesResource.CurrentRadiusUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnTargetModeOptionButtonItemSelected(int index)
	{
		towerAttributesResource.TowerTargetMode = (TowerTargetMode)index;
	}
}
