using Game.Component;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{
	[Signal] public delegate void TowerPlacedEventHandler(BuildingComponent buildingComponent);
	[Signal] public delegate void OpenUpgradeMenuEventHandler(TowerAttributesResource towerAttributesResource, CurrentTowerAttributesResource currentTowerAttributesResource);

	public static GameEvents Instance { get; private set; }

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
			Instance = this;
	}

	public void EmitSignalTowerPlaced(BuildingComponent buildingComponent)
	{
		Instance.EmitSignal(SignalName.TowerPlaced, buildingComponent);
	}

	public void EmitSignalOpenUpgradeMenu(TowerAttributesResource towerAttributesResource, CurrentTowerAttributesResource currentTowerAttributesResource)
	{
		Instance.EmitSignal(SignalName.OpenUpgradeMenu, towerAttributesResource, currentTowerAttributesResource);
	}
}
