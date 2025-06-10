using Godot;

namespace Game.UI;

public partial class TurretPlacementMenu : Control
{
	[Export] public TurretAttributesResource[] TurretAttributesResources { get; private set; }

	[Export(PropertyHint.File, ".tscn")]
	public string TurretPlacementContainerFilePath { get; private set; }

	public override void _Ready()
	{
		foreach (var turret in TurretAttributesResources)
		{
			var scene = GD.Load<PackedScene>(TurretPlacementContainerFilePath);
			var turretPlacementContainer = scene.Instantiate<TurretPlacementContainer>();

			turretPlacementContainer.TowerAttributesResource = turret;
			turretPlacementContainer.UpdateAttributes();
			GetNode<VBoxContainer>("%TurretVBoxContainer").AddChild(turretPlacementContainer);
		}
	}
}
