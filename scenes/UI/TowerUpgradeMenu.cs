using Game.Autoload;
using Game.Enums;
using Godot;
using TowerDefense.enums;

namespace Game.UI;

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

	private TurretAttributesResource towerAttributesResource;
	private CurrentTowerAttributesResource currentTowerAttributesResource;

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

	private void OnOpenUpgradeMenu(TurretAttributesResource towerAttributesResource, CurrentTowerAttributesResource currentTowerAttributesResource)
	{
		if (isOpen)
		{
			GD.PrintErr("UpgradeComponent is already open.");
			return;
		}
		this.towerAttributesResource = towerAttributesResource;
		this.currentTowerAttributesResource = currentTowerAttributesResource;
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

		var tierLabelContent = currentTowerAttributesResource.Tier.ToString().Replace("Tier", "").ToLower();
		CurrentTierLabel.Text = $"Current Tier: {tierLabelContent}";

		damageProgressBar.Value = currentTowerAttributesResource.DamageUpgradePercentage;
		fireRateProgressBar.Value = currentTowerAttributesResource.FireRateUpgradePercentage;
		bulletSpeedProgressBar.Value = currentTowerAttributesResource.BulletSpeedUpgradePercentage;
		radiusProgressBar.Value = currentTowerAttributesResource.RadiusUpgradePercentage;

		targetModeOptionButton.Select((int)currentTowerAttributesResource.TowerTargetMode);
	}

	private void OnDamageUpgradeButtonPressed()
	{		
		currentTowerAttributesResource.DamageUpgradePercentage += 20;
		UpdateInfo();

		if (currentTowerAttributesResource.DamageUpgradePercentage == 100)
		{
			GetNode<Button>("%DamageButton").Disabled = true;
		}
	}

	private void OnFireRateUpgradeButtonPressed()
	{
		currentTowerAttributesResource.FireRateUpgradePercentage += 20;
		UpdateInfo();

		if (currentTowerAttributesResource.FireRateUpgradePercentage == 100)
		{
			GetNode<Button>("%FireRateButton").Disabled = true;
		}
	}

	private void OnBulletSpeedUpgradeButtonPressed()
	{
		currentTowerAttributesResource.BulletSpeedUpgradePercentage += 20;
		UpdateInfo();

		if (currentTowerAttributesResource.BulletSpeedUpgradePercentage == 100)
		{
			GetNode<Button>("%BulletSpeedButton").Disabled = true;
		}
	}

	private void OnRangeUpgradeButtonPressed()
	{
		currentTowerAttributesResource.RadiusUpgradePercentage += 20;
		UpdateInfo();

		if (currentTowerAttributesResource.RadiusUpgradePercentage == 100)
		{
			GetNode<Button>("%RangeButton").Disabled = true;
		}
	}

	private void OnTargetModeOptionButtonItemSelected(int index)
	{
		currentTowerAttributesResource.TowerTargetMode = (TowerTargetMode)index;
	}

	private void OnUpgradeTierButtonPressed()
	{
		var nextTier = (int)currentTowerAttributesResource.Tier + 1;
		currentTowerAttributesResource.Tier = (TowerTier)nextTier;

		var tierLabelContent = currentTowerAttributesResource.Tier.ToString().Replace("Tier", "").ToLower();
		CurrentTierLabel.Text = $"Current Tier: {tierLabelContent}";

		if (nextTier == 2)
		{
			GetNode<Button>("%UpgradeTierButton").Disabled = true;
		}
	}
}
