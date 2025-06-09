using Game.Component;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{
	[Signal] public delegate void TurretPlacedEventHandler(BuildingComponent buildingComponent);
	[Signal] public delegate void OpenUpgradeMenuEventHandler(TurretAttributesResource turretAttributesResource, CurrentTurretAttributesResource currentTurretAttributesResource);

	public static GameEvents Instance { get; private set; }

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
			Instance = this;
	}

	public void EmitSignalTurretPlaced(BuildingComponent buildingComponent)
	{
		Instance.EmitSignal(SignalName.TurretPlaced, buildingComponent);
	}

	public void EmitSignalOpenUpgradeMenu(TurretAttributesResource turretAttributesResource, CurrentTurretAttributesResource currentTurretAttributesResource)
	{
		Instance.EmitSignal(SignalName.OpenUpgradeMenu, turretAttributesResource, currentTurretAttributesResource);
	}
}
