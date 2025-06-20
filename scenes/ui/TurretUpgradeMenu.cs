using System;
using Game.Autoload;
using Game.Component;
using Game.Enums;
using Game.Turret;
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
	private TurretManager currentTurret;

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

		var turretTargetModeNames = Enum.GetNames(typeof(TurretTargetMode));
		var turretTargetModeValues = Enum.GetValues(typeof(TurretTargetMode));
		for (int i = 0; i < turretTargetModeValues.Length; i++)
		{
			targetModeOptionButton.AddItem(
				turretTargetModeNames.GetValue(i).ToString(),
				(int)turretTargetModeValues.GetValue(i)
			);
		}
	}

	private void OnOpenUpgradeMenu(TurretAttributesComponent turretAttributesComponent, TurretManager turretManager)
	{
		if (isOpen)
		{
			GD.PrintErr("UpgradeComponent is already open.");
			return;
		}
		turretAttributesResource = turretAttributesComponent.TurretAttributesResource;
		currentTurretAttributesResource = turretAttributesComponent.CurrentTurretAttributesResource;
		currentTurret = turretManager;
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

		UpdateDamageButton();
		UpdateFireRateButton();
		UpdateBulletSpeedButton();
		UpdateRangeButton();
		UpdateUpgradeTierButton();
	}

	private void OnDamageUpgradeButtonPressed()
	{
		currentTurretAttributesResource.DamageUpgradePercentage += 20;
		UpdateInfo();

		UpdateDamageButton();
	}

	private void OnFireRateUpgradeButtonPressed()
	{
		currentTurretAttributesResource.FireRateUpgradePercentage += 20;
		UpdateInfo();

		UpdateFireRateButton();
	}

	private void OnBulletSpeedUpgradeButtonPressed()
	{
		currentTurretAttributesResource.BulletSpeedUpgradePercentage += 20;
		UpdateInfo();

		UpdateBulletSpeedButton();
	}

	private void OnRangeUpgradeButtonPressed()
	{
		currentTurretAttributesResource.RadiusUpgradePercentage += 20;
		UpdateInfo();

		UpdateRangeButton();
	}

	private void OnTargetModeOptionButtonItemSelected(int index)
	{
		currentTurretAttributesResource.TurretTargetMode = (TurretTargetMode)index;
	}

	private void OnUpgradeTierButtonPressed()
	{
		var nextTier = (int)currentTurretAttributesResource.Tier + 1;
		currentTurretAttributesResource.Tier = (TurretTier)nextTier;

		UpdateUpgradeTierButton();
	}

	private void UpdateUpgradeTierButton()
	{
		var button = GetNode<Button>("%UpgradeTierButton");

		if (currentTurretAttributesResource.Tier == TurretTier.TierThree)
		{
			CurrentTierLabel.Text = $"Max Tier reached.";
			button.Disabled = true;
			return;
		}

		var tierLabelContent = currentTurretAttributesResource.Tier.ToString().Replace("Tier", "").ToLower();
		CurrentTierLabel.Text = $"Current Tier: {tierLabelContent}";
		button.Disabled = false;
	}

	private void UpdateDamageButton()
	{
		var button = GetNode<Button>("%DamageButton");
		if (currentTurretAttributesResource.DamageUpgradePercentage == 100)
		{
			button.Disabled = true;
			return;
		}
		button.Disabled = false;
	}

	private void UpdateFireRateButton()
	{
		var button = GetNode<Button>("%FireRateButton");
		if (currentTurretAttributesResource.FireRateUpgradePercentage == 100)
		{
			button.Disabled = true;
			return;
		}
		button.Disabled = false;
	}

	private void UpdateBulletSpeedButton()
	{
		var button = GetNode<Button>("%BulletSpeedButton");
		if (currentTurretAttributesResource.BulletSpeedUpgradePercentage == 100)
		{
			button.Disabled = true;
			return;
		}
		button.Disabled = false;
	}

	private void UpdateRangeButton()
	{
		var button = GetNode<Button>("%RangeButton");
		if (currentTurretAttributesResource.RadiusUpgradePercentage == 100)
		{
			button.Disabled = true;
			return;
		}
		button.Disabled = false;
	}
}
