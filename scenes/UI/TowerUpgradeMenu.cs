using Game.Autoload;
using Game.Enums;
using Godot;
using TurretDefense.enums;

namespace Game.UI;

public partial class TurretUpgradeMenu : Control
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

	private TurretAttributesResource turretAttributesResource;
	private CurrentTurretAttributesResource currentTurretAttributesResource;

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

		targetModeOptionButton.AddItem(TurretTargetMode.First.ToString(), (int)TurretTargetMode.First);
		targetModeOptionButton.AddItem(TurretTargetMode.Last.ToString(), (int)TurretTargetMode.Last);
		targetModeOptionButton.AddItem(TurretTargetMode.Closest.ToString(), (int)TurretTargetMode.Closest);
	}

	private void OnOpenUpgradeMenu(TurretAttributesResource turretAttributesResource, CurrentTurretAttributesResource currentTurretAttributesResource)
	{
		if (isOpen)
		{
			GD.PrintErr("UpgradeComponent is already open.");
			return;
		}
		this.turretAttributesResource = turretAttributesResource;
		this.currentTurretAttributesResource = currentTurretAttributesResource;
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
		NameLabel.Text = turretAttributesResource.Name;
		DescriptionLabel.Text = turretAttributesResource.Description;

		var tierLabelContent = currentTurretAttributesResource.Tier.ToString().Replace("Tier", "").ToLower();
		CurrentTierLabel.Text = $"Current Tier: {tierLabelContent}";

		damageProgressBar.Value = currentTurretAttributesResource.DamageUpgradePercentage;
		fireRateProgressBar.Value = currentTurretAttributesResource.FireRateUpgradePercentage;
		bulletSpeedProgressBar.Value = currentTurretAttributesResource.BulletSpeedUpgradePercentage;
		radiusProgressBar.Value = currentTurretAttributesResource.RadiusUpgradePercentage;

		targetModeOptionButton.Select((int)currentTurretAttributesResource.TurretTargetMode);
	}

	private void OnDamageUpgradeButtonPressed()
	{		
		currentTurretAttributesResource.DamageUpgradePercentage += 20;
		UpdateInfo();

		if (currentTurretAttributesResource.DamageUpgradePercentage == 100)
		{
			GetNode<Button>("%DamageButton").Disabled = true;
		}
	}

	private void OnFireRateUpgradeButtonPressed()
	{
		currentTurretAttributesResource.FireRateUpgradePercentage += 20;
		UpdateInfo();

		if (currentTurretAttributesResource.FireRateUpgradePercentage == 100)
		{
			GetNode<Button>("%FireRateButton").Disabled = true;
		}
	}

	private void OnBulletSpeedUpgradeButtonPressed()
	{
		currentTurretAttributesResource.BulletSpeedUpgradePercentage += 20;
		UpdateInfo();

		if (currentTurretAttributesResource.BulletSpeedUpgradePercentage == 100)
		{
			GetNode<Button>("%BulletSpeedButton").Disabled = true;
		}
	}

	private void OnRangeUpgradeButtonPressed()
	{
		currentTurretAttributesResource.RadiusUpgradePercentage += 20;
		UpdateInfo();

		if (currentTurretAttributesResource.RadiusUpgradePercentage == 100)
		{
			GetNode<Button>("%RangeButton").Disabled = true;
		}
	}

	private void OnTargetModeOptionButtonItemSelected(int index)
	{
		currentTurretAttributesResource.TurretTargetMode = (TurretTargetMode)index;
	}

	private void OnUpgradeTierButtonPressed()
	{
		var nextTier = (int)currentTurretAttributesResource.Tier + 1;
		currentTurretAttributesResource.Tier = (TurretTier)nextTier;

		var tierLabelContent = currentTurretAttributesResource.Tier.ToString().Replace("Tier", "").ToLower();
		CurrentTierLabel.Text = $"Current Tier: {tierLabelContent}";

		if (nextTier == 2)
		{
			GetNode<Button>("%UpgradeTierButton").Disabled = true;
		}
	}
}
