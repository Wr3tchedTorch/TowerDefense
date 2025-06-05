using Game.Autoload;
using Game.Enums;
using Godot;

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

	private TowerAttributesResource towerAttributesResource;
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

	private void OnOpenUpgradeMenu(TowerAttributesResource towerAttributesResource, CurrentTowerAttributesResource currentTowerAttributesResource)
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
		CurrentTierLabel.Text = $"Current Tier: {currentTowerAttributesResource.Tier}";

		damageProgressBar.Value = currentTowerAttributesResource.DamageUpgradePercentage;
		fireRateProgressBar.Value = currentTowerAttributesResource.FireRateUpgradePercentage;
		bulletSpeedProgressBar.Value = currentTowerAttributesResource.BulletSpeedUpgradePercentage;
		radiusProgressBar.Value = currentTowerAttributesResource.RadiusUpgradePercentage;

		targetModeOptionButton.Select((int)currentTowerAttributesResource.TowerTargetMode);
	}

	private void OnDamageUpgradeButtonPressed()
	{
		if (currentTowerAttributesResource.DamageUpgradePercentage == 100)
		{
			GD.Print("Maximum damage upgrade reached.");
			return;
		}
		currentTowerAttributesResource.DamageUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnFireRateUpgradeButtonPressed()
	{
		if (currentTowerAttributesResource.FireRateUpgradePercentage == 100)
		{
			GD.Print("Maximum fire rate upgrade reached.");
			return;
		}
		currentTowerAttributesResource.FireRateUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnBulletSpeedUpgradeButtonPressed()
	{
		if (currentTowerAttributesResource.BulletSpeedUpgradePercentage == 100)
		{
			GD.Print("Maximum bullet speed upgrade reached.");
			return;
		}
		currentTowerAttributesResource.BulletSpeedUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnRangeUpgradeButtonPressed()
	{
		if (currentTowerAttributesResource.RadiusUpgradePercentage == 100)
		{
			GD.Print("Maximum range upgrade reached.");
			return;
		}
		currentTowerAttributesResource.RadiusUpgradePercentage += 20;
		UpdateInfo();
	}

	private void OnTargetModeOptionButtonItemSelected(int index)
	{
		currentTowerAttributesResource.TowerTargetMode = (TowerTargetMode)index;
	}
}
