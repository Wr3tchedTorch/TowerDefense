	using Game.Component;
	using Godot;

	namespace Game.Autoload;

	public partial class GameEvents : Node
	{
		[Signal] public delegate void TurretPlacedEventHandler(BuildingComponent buildingComponent);
		[Signal] public delegate void OpenUpgradeMenuEventHandler(TurretAttributesResource turretAttributesResource, CurrentTurretAttributesResource currentTurretAttributesResource);
		[Signal] public delegate void TogglePlacementMenuEventHandler();
		[Signal] public delegate void TurretBoughtEventHandler(TurretAttributesResource turretResource);

		public static GameEvents Instance { get; private set; }

		public override void _Notification(int what)
		{		
			if (what == NotificationSceneInstantiated)
				Instance = this;
		}
	}