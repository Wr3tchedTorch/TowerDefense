using Game.Component;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{
	[Signal] public delegate void TowerPlacedEventHandler(BuildingComponent buildingComponent);
	[Signal] public delegate void OpenUpgradeMenuEventHandler(TowerResource towerResource);

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

	public void EmitSignalOpenUpgradeMenu(TowerResource towerResource)
	{
		Instance.EmitSignal(SignalName.OpenUpgradeMenu, towerResource);
	}
}
