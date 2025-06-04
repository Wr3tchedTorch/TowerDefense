using Game.Autoload;
using Godot;

public partial class TowerUpgradeMenu : Control
{
	private bool isOpen = false;

	public override void _Ready()
	{
		GameEvents.Instance.OpenUpgradeMenu += OnOpenUpgradeMenu;
	}

	private void OnOpenUpgradeMenu(TowerAttributesResource towerAttributesResource)
	{
		GD.Print($"Opening upgrade menu for tower: {towerAttributesResource.Name}");
		GD.Print($"Current Tier: {towerAttributesResource.CurrentTier}");
		GD.Print($"Damage: {towerAttributesResource.Damage}");
		GD.Print($"Fire Rate: {towerAttributesResource.FireRate}");
		GD.Print($"Bullet Speed: {towerAttributesResource.BulletSpeed}");
		GD.Print($"Radius: {towerAttributesResource.Radius}");

		if (isOpen)
		{
			GD.PrintErr("UpgradeComponent is already open.");
			return;
		}
		Visible = true;
		isOpen = true;
	}

	private void OnCloseButtonPressed()
	{
		Visible = false;
		isOpen = false;
	}
}
