using Game.Autoload;
using Godot;

namespace Game.UI;

public partial class TurretPlacementMenu : Control
{
	[Export] public TurretAttributesResource[] TurretAttributesResources { get; private set; }

	[Export(PropertyHint.File, ".tscn")]
	public string TurretPlacementContainerFilePath { get; private set; }

	private PanelContainer menuPanelContainer;
	private Button toggleButton;

	public override void _Ready()
	{
		menuPanelContainer = GetNode<PanelContainer>("%MenuPanelContainer");
		toggleButton = GetNode<Button>("%ToggleButton");

		menuPanelContainer.Visible = false;
		toggleButton.Text = "<<";

		GameEvents.Instance.TogglePlacementMenu += Toggle;
		toggleButton.Pressed += OnToggleMenuPressed;

		foreach (var turret in TurretAttributesResources)
		{
			var scene = GD.Load<PackedScene>(TurretPlacementContainerFilePath);
			var turretPlacementContainer = scene.Instantiate<TurretPlacementContainer>();

			turretPlacementContainer.TowerAttributesResource = turret;
			turretPlacementContainer.UpdateAttributes();
			GetNode<VBoxContainer>("%TurretVBoxContainer").AddChild(turretPlacementContainer);
		}
	}

	public void Toggle()
	{
		menuPanelContainer.Visible = !menuPanelContainer.Visible;
		toggleButton.Text = menuPanelContainer.Visible ? ">>" : "<<";
	}

	public void OnToggleMenuPressed()
	{
		Toggle();
	}
}
